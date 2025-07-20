package com.wc.filetransfer;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ProgressBar;
import android.widget.Spinner;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private final UDPDiscovery udpDiscovery = new UDPDiscovery();
    private final FileReceiver fileServer = new FileReceiver(this);
    private FileSender fileSender;
    private final int PICKFILE_REQUEST_CODE = 100;
    private final int PICKFOLDER_REQUEST_CODE = 1001;
    private Button btn_repond;
    private Button btn_start_nano;
    private Button btn_search;
    private Button btn_stop_nano;
    private Spinner currentDevices;
    private List<String> rawResponse = new ArrayList<>();
    private List<String> respodedDevices = new ArrayList<>();
    private List<String> respondDeviceIps = new ArrayList<>();
    private int selectedSpinnerIndex =-1;
    public static Uri uri;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });

        btn_repond = findViewById(R.id.btn_broadcast_respons);
        btn_start_nano = findViewById(R.id.btn_nanoHttpd_on);
        btn_stop_nano = findViewById(R.id.btn_nanoHttpd_off);
        ProgressBar progressBar = findViewById(R.id.progressBar);
        fileSender = new FileSender(this,progressBar);

        btn_repond.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                udpDiscovery.startDiscoveryListener();
            }
        });

        btn_start_nano.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                fileServer.startServer();
                Toast.makeText(MainActivity.this, "Server Started", Toast.LENGTH_SHORT).show();
                btn_start_nano.setEnabled(false);
                btn_stop_nano.setEnabled(true);
            }
        });

        btn_stop_nano.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                    fileServer.stopServer();
                    Toast.makeText(MainActivity.this, "Server Stopped", Toast.LENGTH_SHORT).show();
                    btn_start_nano.setEnabled(true);
                    btn_stop_nano.setEnabled(false);
            }
        });
    }

    public void sendFile(View view){
        try{
        AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
        alertDialog.setTitle("Do want to send a File or a Folder")
                .setItems(new CharSequence[]{"Pick a File","Pick a Folder"},(dialog,which)->{
                    if(which == 0){
                        Intent pickFileIntent = new Intent(Intent.ACTION_OPEN_DOCUMENT);
                        pickFileIntent.setType("*/*");
                        pickFileIntent.addCategory(Intent.CATEGORY_OPENABLE);
                        startActivityForResult(pickFileIntent,PICKFILE_REQUEST_CODE);
                    } else if (which ==1) {
                       Intent pickFolderIntent = new Intent(Intent.ACTION_OPEN_DOCUMENT_TREE);
                        startActivityForResult(pickFolderIntent,PICKFOLDER_REQUEST_CODE);
                    }
                });
        selectedSpinnerIndex = currentDevices.getSelectedItemPosition();
        alertDialog.show();
        }
        catch (Exception e){
            Log.d("MainActivity",e.getMessage());
        }
    }

    public void btn_search_click(View view){
        try {
            respodedDevices.clear(); //clear out the list
            btn_search = findViewById(R.id.btn_search);
            currentDevices = findViewById(R.id.spinnerDevices);
            udpDiscovery.discoverDevices(devices -> runOnUiThread(() -> {
                rawResponse = devices;
                cleanResponse(rawResponse);

                ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_dropdown_item, respodedDevices);
                currentDevices.setAdapter(adapter);

            }));
        }catch (Exception e){
            Log.d("MainActivity",e.getMessage());
        }
    }


    private void cleanResponse(List<String>raw){
        int count = 0;
        String rawValue;
        try {
            while (!raw.isEmpty() && count < raw.size()) {
                rawValue = raw.get(count);
                int speraterIndex = rawValue.indexOf("|");
                String deviceName = rawValue.substring(0, speraterIndex);
                String relatedIp = rawValue.substring(speraterIndex + 1);

                respodedDevices.add(deviceName);
                respondDeviceIps.add(relatedIp);
                count++;
            }
        }catch (Exception e){
            Log.d("MainActivity",e.getMessage());
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        try {
            if (resultCode == RESULT_OK && data != null) {
                   uri = data.getData();
                if(requestCode == PICKFILE_REQUEST_CODE){
                    fileSender.sendFileToDevice(uri,respondDeviceIps.get(selectedSpinnerIndex));
                }
                else if(requestCode == PICKFOLDER_REQUEST_CODE){
                    fileSender.sendFileToDevice(uri,respondDeviceIps.get(selectedSpinnerIndex));
                }
            }
        }catch (Exception e){
            Log.d("MainActivity",e.getMessage());
        }

    }
}