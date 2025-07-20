package com.wc.filetransfer;

import android.app.Activity;
import android.content.ContentResolver;
import android.content.Context;
import android.database.Cursor;
import android.net.Uri;
import android.provider.OpenableColumns;
import android.util.Log;
import android.widget.ProgressBar;

import androidx.appcompat.app.AppCompatActivity;

import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.nio.charset.StandardCharsets;

public class FileSender {

    private final Context context;
    private final ProgressBar progressBar;

    public FileSender(Context context,ProgressBar progressBar) {
        this.context = context;
        this.progressBar = progressBar;
    }

    public void sendFileToDevice(Uri uri, String encryptedServerIp) {
        new Thread(() -> {
            try {
                String serverIp = EncryptionHelper.decrypt(encryptedServerIp);
                ContentResolver myContent = context.getContentResolver();

                Cursor fileCursor = myContent.query(uri, null, null, null, null);

                if (fileCursor == null || !fileCursor.moveToFirst()) {
                    Log.e("FileSender", "Failed to get file info from Uri");
                    return;
                }

                int nameIndex = fileCursor.getColumnIndex(OpenableColumns.DISPLAY_NAME);
                String fileName = fileCursor.getString(nameIndex);
                int fileSizeIndex = fileCursor.getColumnIndex(OpenableColumns.SIZE);
                long actualSize = fileCursor.getLong(fileSizeIndex);
                if (actualSize <= 0) actualSize = 1;
                fileCursor.close();

                try (Socket socket = new Socket(serverIp, 5000);
                     OutputStream outputStream = socket.getOutputStream();
                     InputStream inputStream = myContent.openInputStream(uri)) {

                    if (inputStream == null) {
                        Log.e("FileSender", "Cannot open InputStream from Uri");
                        return;
                    }

                    byte[] fileNameBytes = fileName.getBytes(StandardCharsets.UTF_8);

                    // Write 2-byte big-endian filename length
                    outputStream.write((fileNameBytes.length >> 8) & 0xFF);
                    outputStream.write(fileNameBytes.length & 0xFF);

                    // Write filename bytes
                    outputStream.write(fileNameBytes);

                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    long totalSent = 0;

                    while ((bytesRead = inputStream.read(buffer)) != -1) {
                        outputStream.write(buffer, 0, bytesRead);
                        totalSent += bytesRead;
                        int progress = (int) ((totalSent * 100) / actualSize);
                        updateProgress(progress);
                        Log.d("FileSender", "Sent " + bytesRead + " bytes (Total: " + totalSent + ")");
                    }

                    outputStream.flush();
                    Log.d("FileSender", "File sent successfully: " + fileName);
                    updateProgress(0);

                } catch (Exception e) {
                    Log.e("FileSender", "Error sending file", e);
                    updateProgress(0);
                }

            } catch (Exception e) {
                Log.e("FileSender", "Decryption or socket error", e);
                updateProgress(0);
            }
        }).start();
    }

    private void updateProgress(int value) {
        if (context instanceof Activity) {
            ((Activity) context).runOnUiThread(() -> progressBar.setProgress(value));
        }
    }
}
