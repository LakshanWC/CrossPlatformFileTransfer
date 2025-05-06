package com.wc.filetransfer;

import android.content.Context;
import android.os.Environment;
import android.util.Log;

import fi.iki.elonen.NanoHTTPD;

import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.util.Map;

public class FileServer extends NanoHTTPD {

    private static final String TAG = "FileServer";
    private boolean isRunning = false;
    private Context context;

    public FileServer(Context context) {
        super(8080);
        this.context = context;
    }

    @Override
    public Response serve(IHTTPSession session) {
        if (Method.POST.equals(session.getMethod())) {
            try {
                Map<String, String> files = new java.util.HashMap<>();
                session.parseBody(files);

                String fileName = session.getParms().get("filename");
                String tmpFilePath = files.get("file");

                File downloadsDir = Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_DOWNLOADS);
                File targetFile = new File(downloadsDir, fileName);

                try (InputStream inputStream = session.getInputStream();
                     FileOutputStream outputStream = new FileOutputStream(targetFile)) {

                    byte[] buffer = new byte[4096];
                    int bytesRead;

                    while ((bytesRead = inputStream.read(buffer)) != -1) {
                        outputStream.write(buffer, 0, bytesRead);
                    }
                }

                // Show toast on main thread
                new android.os.Handler(android.os.Looper.getMainLooper()).post(() ->
                        android.widget.Toast.makeText(context, "File received: " + fileName, android.widget.Toast.LENGTH_SHORT).show()
                );

                return newFixedLengthResponse(Response.Status.OK, "text/plain", "File received: " + fileName);

            } catch (Exception e) {
                Log.e(TAG, "Error receiving file", e);
                return newFixedLengthResponse(Response.Status.INTERNAL_ERROR, "text/plain", "Error: " + e.getMessage());
            }
        }

        return newFixedLengthResponse("File server is running.");
    }

    //  Start the server safely
    public void startServer() {
        if (!isRunning) {
            try {
                start();
                isRunning = true;
                Log.d(TAG, "File server started on port " + getListeningPort());
            } catch (Exception e) {
                Log.e(TAG, "Failed to start file server", e);
            }
        }
    }

    // stop the server safely
    public void stopServer() {
        if (isRunning) {
            stop();
            isRunning = false;
            Log.d(TAG, "File server stopped.");
        }
    }

    public boolean isRunning() {
        return isRunning;
    }
}
