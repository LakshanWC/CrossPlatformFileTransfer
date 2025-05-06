package com.wc.filetransfer;

import android.util.Log;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.util.Collections;
import java.util.List;

public class UDPDiscovery {

    private String b;
    private String deviceIp;

    public void startDiscoveryListener(){

        deviceIp = getIPAddress(true);
        deviceIp = EncryptionHelper.encrypt(deviceIp);


        Thread thread = new Thread(() ->{
            try{
                DatagramSocket socket = new DatagramSocket(8888, InetAddress.getByName("0.0.0.0"));
                socket.setBroadcast(true);
                byte[] recvBuf = new byte[15000];

                Log.d("Message","sending");

                while (true) {
                    DatagramPacket packet = new DatagramPacket(recvBuf, recvBuf.length);
                    socket.receive(packet);

                    String message = new String(packet.getData(), 0, packet.getLength());

                    Log.d("Message","Finding");
                    if ("DISCOVER_FILETRANSFER".equals(message)) {
                        String response = "ANDROID_DEVICE:" + android.os.Build.MODEL + "|"+deviceIp;
                        byte[] responseData = response.getBytes();

                        DatagramPacket responsePacket = new DatagramPacket(
                                responseData, responseData.length,
                                packet.getAddress(), packet.getPort()
                        );
                        socket.send(responsePacket);
                    }
                }
            }catch (Exception e){
                Log.d("Error",e.getMessage());
            }
        });

        thread.start();
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
}
