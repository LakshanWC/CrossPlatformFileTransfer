package com.wc.filetransfer;

import android.os.Bundle;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Spinner;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import java.util.ArrayList;
import java.util.List;

public class MainActivity extends AppCompatActivity {

    private UDPDiscovery udpDiscovery = new UDPDiscovery();
    private FileReceiver fileServer = new FileReceiver(this);
    private Button btn_repond;
    private Button btn_start_nano;
    private Button btn_stop_nano;

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
        udpDiscovery.discoverDevices(updatedList -> runOnUiThread(() -> {
            Spinner deviceSpinner = findViewById(R.id.spinnerDevices);
            ArrayAdapter<String> adapter = new ArrayAdapter<>(
                    this, android.R.layout.simple_spinner_item, updatedList);
            adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            deviceSpinner.setAdapter(adapter);
        }));
    }
}