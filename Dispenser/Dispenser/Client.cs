using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public static class Client
{

    public static string StartClient(string data, string ip)
    {
        // Data buffer for incoming data.
        string result = "";
        byte[] bytes = new byte[1024];

        // Connect to a remote device.
        try
        {
            // Establish the remote endpoint for the socket.
            // This example uses port 11000 on the local computer.
            IPHostEntry ipHostInfo = Dns.Resolve(ip);
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP  socket.
            Socket sender = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.
            try
            {
                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.
                byte[] msg = Encoding.ASCII.GetBytes(data + "<EOF>");

                // Send the data through the socket.
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.
                int bytesRec = sender.Receive(bytes);
                Console.WriteLine("Echoed test = {0}",
                    Encoding.ASCII.GetString(bytes, 0, bytesRec));

                // Release the socket.
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                result = "success";
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                result = String.Format("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Console.WriteLine("SocketException : {0}", se.ToString());
                result = String.Format("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
                result = String.Format("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            result = e.ToString();
        }
        return result;
    }

    
}