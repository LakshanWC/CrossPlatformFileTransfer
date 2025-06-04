package com.wc.filetransfer;

import android.content.Context;
import android.content.ContentValues;
import android.net.Uri;
import android.os.Build;
import android.os.Environment;
import android.provider.MediaStore;
import android.util.Log;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.nio.charset.StandardCharsets;
import java.util.concurrent.atomic.AtomicBoolean;
import java.util.HashMap;

public class FileReceiver {

    private static final int PORT = 5000;
    private static final String TAG = "FileReceiver";
    private static final HashMap<String, String> MIME_TYPES = new HashMap<String, String>() {{
        put("jpg", "image/jpeg");
        put("jpeg", "image/jpeg");
        put("png", "image/png");
        put("gif", "image/gif");
        put("pdf", "application/pdf");
        put("doc", "application/msword");
        put("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        put("xls", "application/vnd.ms-excel");
        put("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        put("ppt", "application/vnd.ms-powerpoint");
        put("pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation");
        put("zip", "application/zip");
        put("rar", "application/x-rar-compressed");
        put("mp3", "audio/mpeg");
        put("mp4", "video/mp4");
        put("txt", "text/plain");
    }};

    private final Context context;
    private final AtomicBoolean isRunning = new AtomicBoolean(false);

    private ServerSocket serverSocket;
    private Socket clientSocket;
    private InputStream inputStream;
    private OutputStream fileOutputStream;
    private Thread serverThread;

    public FileReceiver(Context context) {
        this.context = context;
    }

    public void startServer() {
        if (isRunning.get()) {
            Log.w(TAG, "Server is already running");
            return;
        }

        isRunning.set(true);
        serverThread = new Thread(() -> {
            try {
                serverSocket = new ServerSocket(PORT);
                Log.d(TAG, "Server started on port " + PORT);

                while (isRunning.get()) {
                    try {
                        clientSocket = serverSocket.accept();
                        Log.d(TAG, "Connection established with: " + clientSocket.getInetAddress());

                        inputStream = clientSocket.getInputStream();

                        // Read filename length (2 bytes, big-endian)
                        byte[] lengthBytes = new byte[2];
                        readFully(inputStream, lengthBytes);
                        int fileNameLength = ((lengthBytes[0] & 0xff) << 8) | (lengthBytes[1] & 0xff);

                        // Read filename
                        byte[] fileNameBytes = new byte[fileNameLength];
                        readFully(inputStream, fileNameBytes);
                        String fileName = new String(fileNameBytes, StandardCharsets.UTF_8);
                        Log.d(TAG, "Receiving file: " + fileName);

                        // Get MIME type
                        String fileExtension = getFileExtension(fileName);
                        String mimeType = MIME_TYPES.getOrDefault(fileExtension.toLowerCase(), "application/octet-stream");

                        // Create output file
                        fileOutputStream = getDownloadFolderOutputStream(fileName, mimeType);
                        Log.d(TAG, "Saving file as: " + fileName + " (Type: " + mimeType + ")");

                        // Receive file data
                        byte[] buffer = new byte[1024 * 1024];//4096
                        int bytesRead;
                        long totalBytes = 0;

                        while ((bytesRead = inputStream.read(buffer)) != -1 && isRunning.get()) {
                            fileOutputStream.write(buffer, 0, bytesRead);
                            totalBytes += bytesRead;
                            Log.d(TAG, "Received " + bytesRead + " bytes (Total: " + totalBytes + ")");
                        }

                        if (isRunning.get()) {
                            Log.d(TAG, "File transfer completed successfully. Total bytes: " + totalBytes);
                        } else {
                            Log.d(TAG, "File transfer stopped by user");
                        }

                    } catch (IOException e) {
                        if (isRunning.get()) {
                            Log.e(TAG, "Error during file transfer", e);
                        }
                    } finally {
                        closeResources();
                    }
                }
            } catch (IOException e) {
                Log.e(TAG, "Server error", e);
            } finally {
                isRunning.set(false);
                closeServerSocket();
                Log.d(TAG, "Server stopped");
            }
        });
        serverThread.start();
    }

    public void stopServer() {
        isRunning.set(false);
        closeResources();
        closeServerSocket();

        if (serverThread != null) {
            serverThread.interrupt();
        }
        Log.d(TAG, "Server stop requested");
    }

    private String getFileExtension(String filename) {
        int lastDot = filename.lastIndexOf('.');
        if (lastDot == -1) return "";
        return filename.substring(lastDot + 1);
    }

    private OutputStream getDownloadFolderOutputStream(String fileName, String mimeType) throws IOException {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.Q) {
            ContentValues contentValues = new ContentValues();
            contentValues.put(MediaStore.MediaColumns.DISPLAY_NAME, fileName);
            contentValues.put(MediaStore.MediaColumns.MIME_TYPE, mimeType);
            contentValues.put(MediaStore.MediaColumns.RELATIVE_PATH, Environment.DIRECTORY_DOWNLOADS);

            Uri uri = context.getContentResolver().insert(MediaStore.Downloads.EXTERNAL_CONTENT_URI, contentValues);
            if (uri == null) {
                throw new IOException("Failed to create file in Downloads folder");
            }
            return context.getContentResolver().openOutputStream(uri);
        } else {
            File downloadsDir = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS);
            if (!downloadsDir.exists() && !downloadsDir.mkdirs()) {
                throw new IOException("Failed to create Downloads directory");
            }
            File file = new File(downloadsDir, fileName);
            return new FileOutputStream(file);
        }
    }

    private void closeResources() {
        try {
            if (inputStream != null) {
                inputStream.close();
                Log.d(TAG, "Input stream closed");
            }
            if (fileOutputStream != null) {
                fileOutputStream.close();
                Log.d(TAG, "Output stream closed");
            }
            if (clientSocket != null) {
                clientSocket.close();
                Log.d(TAG, "Client socket closed");
            }
        } catch (IOException e) {
            Log.e(TAG, "Error closing resources", e);
        }
    }

    private void closeServerSocket() {
        try {
            if (serverSocket != null && !serverSocket.isClosed()) {
                serverSocket.close();
                Log.d(TAG, "Server socket closed");
            }
        } catch (IOException e) {
            Log.e(TAG, "Error closing server socket", e);
        }
    }

    private void readFully(InputStream in, byte[] buffer) throws IOException {
        int bytesRead = 0;
        while (bytesRead < buffer.length) {
            int count = in.read(buffer, bytesRead, buffer.length - bytesRead);
            if (count == -1) {
                throw new EOFException("Unexpected end of stream");
            }
            bytesRead += count;
        }
    }
}