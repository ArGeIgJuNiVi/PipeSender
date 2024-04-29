using System.Diagnostics;
using System.IO.Pipes;

namespace PipeSender;

public class _
{
    public static void Main(string[] args)
    {
        using AnonymousPipeServerStream pipe = new(PipeDirection.Out, HandleInheritability.Inheritable);

        Process client = new();
        client.StartInfo.UseShellExecute = false;
        client.StartInfo.FileName = "../PipeReceiver/bin/Debug/net8.0/PipeReceiver";
        client.StartInfo.Arguments = pipe.GetClientHandleAsString();
        client.Start();

        byte[] pipedValue = [.. string.Join("", args).ToCharArray().Select((val) => (byte)val)];
        pipe.Write(BitConverter.GetBytes(pipedValue.Length));
        pipe.Write(pipedValue);

        client.WaitForExit();
        client.Close();
    }
}