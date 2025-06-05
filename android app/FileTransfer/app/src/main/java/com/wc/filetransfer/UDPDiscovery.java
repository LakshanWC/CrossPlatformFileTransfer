package com.wc.filetransfer;

import android.util.Log;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;
import java.util.function.Consumer;

public class UDPDiscovery {

    private String b;
    private String deviceIp;
    private ArrayList<String> deviceInfoList = new ArrayList<>();

    public void discoverDevices(Consumer<List<String>> onDevicesFound) {
        deviceInfoList.clear();
        new Thread(() -> {
            try {
                DatagramSocket datagramSocket = new DatagramSocket();
                datagramSocket.setBroadcast(true);

                byte[] sendMessage = "DISCOVER_FILETRANSFER".getBytes();

                DatagramPacket datagramPacket = new DatagramPacket(sendMessage, sendMessage.length,
                        InetAddress.getByName("255.255.255.255"), 8888);

                datagramSocket.send(datagramPacket);
                Log.d("UDP", "Discovery packet sent");

                byte[] recvBuf = new byte[15000];
                DatagramPacket receivePacket = new DatagramPacket(recvBuf, recvBuf.length);
                datagramSocket.setSoTimeout(3000); // Wait up to 3 seconds for replies

                long startTime = System.currentTimeMillis();
                while (System.currentTimeMillis() - startTime < 3000) {  // Wait for 3 seconds
                    try {
                        datagramSocket.receive(receivePacket);
                        String response = new String(receivePacket.getData(), 0, receivePacket.getLength());

                        Log.d("UDP", "Received response: " + response);

                        if (!deviceInfoList.contains(response)) {
                            deviceInfoList.add(response);
                            if (onDevicesFound != null) {
                                onDevicesFound.accept(new ArrayList<>(deviceInfoList));
                            }
                        }
                    } catch (Exception e) {
                        // Timeout or no more packets
                        break;
                    }
                }
                datagramSocket.close();
            } catch (Exception e) {
                Log.e("UDP", "Error discovering devices: " + e.getMessage());
            }
        }).start();
    }

    public void startDiscoveryListener() {

        deviceIp = getIPAddress(true);
        Log.d("Discovery", "Original IP: " + deviceIp);

        deviceIp = EncryptionHelper.encrypt(deviceIp);
        Log.d("Discovery", "Encrypted IP: " + deviceIp);

        Thread thread = new Thread(() -> {
            try {
                DatagramSocket socket = new DatagramSocket(8888, InetAddress.getByName("0.0.0.0"));
                socket.setBroadcast(true);
                byte[] recvBuf = new byte[15000];

                Log.d("Discovery", "Socket created, listening for broadcast packets...");

                while (true) {
                    DatagramPacket packet = new DatagramPacket(recvBuf, recvBuf.length);
                    socket.receive(packet);

                    String message = new String(packet.getData(), 0, packet.getLength());
                    Log.d("Discovery", "Packet received from: " + packet.getAddress().getHostAddress());
                    Log.d("Discovery", "Message received: " + message);

                    if ("DISCOVER_FILETRANSFER".equals(message)) {
                        Log.d("Discovery", "Valid discovery message received");

                        String response = "ANDROID_DEVICE:" + android.os.Build.MODEL + "|" + deviceIp;
                        byte[] responseData = response.getBytes();

                        DatagramPacket responsePacket = new DatagramPacket(
                                responseData, responseData.length,
                                packet.getAddress(), packet.getPort()
                        );

                        socket.send(responsePacket);
                        Log.d("Discovery", "Response sent: " + response);
                    } else {
                        Log.d("Discovery", "Received unknown message: " + message);
                    }
                }
            } catch (Exception e) {
                Log.e("DiscoveryError", "Exception in discovery thread", e);
            }
        });

        thread.start();
        Log.d("Discovery", "Discovery listener thread started");
    }



    public static String getIPAddress(boolean useIPv4) {
        try {
            List<NetworkInterface> interfaces = Collections.list(NetworkInterface.getNetworkInterfaces());
            for (NetworkInterface intf : interfaces) {
                List<InetAddress> addrs = Collections.list(intf.getInetAddresses());
                for (InetAddress addr : addrs) {
                    if (!addr.isLoopbackAddress()) {
                        String sAddr = addr.getHostAddress();
                        //boolean isIPv4 = InetAddressUtils.isIPv4Address(sAddr);
                        boolean isIPv4 = sAddr.indexOf(':')<0;

                        if (useIPv4) {
                            if (isIPv4)
                                return sAddr;
                        } else {
                            if (!isIPv4) {
                                int delim = sAddr.indexOf('%'); // drop ip6 zone suffix
                                return delim<0 ? sAddr.toUpperCase() : sAddr.substring(0, delim).toUpperCase();
                            }
                        }
                    }
                }
            }
        } catch (Exception ignored) { } // for now eat exceptions
        return "";
    }

    public ArrayList<String> getDeviceInfoList(){
        return new ArrayList<>(deviceInfoList);
    }
}
