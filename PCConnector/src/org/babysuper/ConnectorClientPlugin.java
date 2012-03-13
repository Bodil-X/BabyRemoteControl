package org.babysuper;

import android.content.Context;
import android.util.Log;
import com.phonegap.api.Plugin;
import com.phonegap.api.PluginResult;
import org.json.JSONArray;
import org.json.JSONException;

import java.io.BufferedWriter;
import java.io.IOException;
import java.io.OutputStreamWriter;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 * Created by Bodil.
 * User: Bodil
 * Date: 12-3-7
 * Time: 下午5:21
 */
public class ConnectorClientPlugin extends Plugin {

    Socket socketClient;
    PrintWriter pWriter;
    Context context;

    @Override
    public PluginResult execute(String actionName, JSONArray jsonParams, String callBack) {
        PluginResult pluginResult = null;
        if ("connect".equals(actionName)) {
            try {
                Log.i("ConnectorClientPlugin", "run the plugin now.");

                String result;
                String serverIp = jsonParams.getString(0);
                int port = jsonParams.getInt(1);
                createConnector(serverIp, port);
                result = "Success";
                pluginResult = new PluginResult(PluginResult.Status.OK, result);
            } catch (Exception ex) {
                if (ex instanceof JSONException)
                    pluginResult = new PluginResult(PluginResult.Status.JSON_EXCEPTION, "missing argument name");
                else if (ex instanceof UnknownHostException)
                    pluginResult = new PluginResult(PluginResult.Status.IO_EXCEPTION, "unknown host.");
                else
                    pluginResult = new PluginResult(PluginResult.Status.IO_EXCEPTION, "IO exception.");
            }
        } else if ("send".equals(actionName)) {
            try {
                String messageStr = jsonParams.getString(0);
                sendMessage(messageStr);
                pluginResult = new PluginResult(PluginResult.Status.OK, "Success");
            } catch (Exception ex) {
                if (ex instanceof JSONException)
                    pluginResult = new PluginResult(PluginResult.Status.JSON_EXCEPTION, "missing argument name");
                else
                    pluginResult = new PluginResult(PluginResult.Status.IO_EXCEPTION, "IO exception.");
            }
        } else if ("close".equals(actionName)) {
            closeConnector();
            pluginResult = new PluginResult(PluginResult.Status.OK, "Success");
        } else
            pluginResult = new PluginResult(PluginResult.Status.INVALID_ACTION, String.format("This Action:{0} is invalid.", actionName));
        return pluginResult;
    }

    private void createConnector(String serverIP, int port) throws IOException {
        InetAddress serverAddress;
        try {
            serverAddress = InetAddress.getByName(serverIP);
        } catch (UnknownHostException ex) {
            throw ex;
        }

        try {
            socketClient = new Socket(serverAddress, port);
            pWriter = new PrintWriter(new BufferedWriter(new OutputStreamWriter(socketClient.getOutputStream())), true);
        } catch (IOException ex) {
            throw ex;
        }
    }

    private void sendMessage(String messageStr) throws IOException {
        pWriter.println(messageStr);
    }

    private boolean closeConnector() {
        pWriter.println("Client Already Close");
        pWriter.println("over");
        pWriter.close();
        return true;
    }
}
