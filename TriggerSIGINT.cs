using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
	static void Main()
	{
		// Replace "node" with the actual process name or identifier of your Node.js process.
		string batchFileName = "start_node.bat";
        int nodeProcessId;
        if (batchFileName != null)
        {
            nodeProcessId = StartNodeJS(batchFileName);
        }
        else
        {
            nodeProcessId = StartNodeJS();
        }
		       
            // Get the process ID (PID) of the Node.js process.
            //int nodeProcessId = nodeProcesses[1].Id;

            // Send the SIGINT signal to the Node.js process.
            Process nodeProcess = Process.GetProcessById(nodeProcessId);

            SendCtrlC(nodeProcessId);
            Dispose(nodeProcess);

    }

	static int StartNodeJS()
	{
        string nodejsExecutable = "node";

        // Replace "path/to/your/nodejs_script.js" with the actual path to your Node.js script.
        string nodejsScript = "eventexitnode.mjs";

        string currentDirectory = Environment.CurrentDirectory;

        string twoFoldersUp = System.IO.Path.GetDirectoryName(System.IO.Directory.GetParent(currentDirectory).FullName);

        // Combine the current directory path with the Node.js script name.
        string nodejsScriptPath = System.IO.Path.Combine(twoFoldersUp, nodejsScript);


        // Create a new process to execute the Node.js script.
        Process process = new Process();
        process.StartInfo.FileName = nodejsExecutable;
        process.StartInfo.Arguments = nodejsScriptPath;

        // Redirect standard output to capture the Node.js script's output.
        process.StartInfo.RedirectStandardOutput = true;
        // Enable the UseShellExecute option to execute the process in its own window.
        process.StartInfo.UseShellExecute = true;

		process.StartInfo.CreateNoWindow = false;
        process.StartInfo.RedirectStandardOutput = true;
        // Start the process.
        process.Start();

		var pid = process.Id;
		return pid;
    }

    static int StartNodeJS(string batchFileName)
    {
        string currentDirectory = Environment.CurrentDirectory;

        string twoFoldersUp = System.IO.Path.GetDirectoryName(System.IO.Directory.GetParent(currentDirectory).FullName);

        // Combine the current directory path with the Node.js script name.
        string BatchFilePath = System.IO.Path.Combine(twoFoldersUp, batchFileName);

        // Create a new process to execute the batch file.
        Process process = new Process();
        process.StartInfo.FileName = BatchFilePath;

        // Enable the UseShellExecute option to execute the batch file in its own window.
        process.StartInfo.UseShellExecute = true;
        // Start the process.
        process.Start();
        // Wait for the process to enter an idle state.
        Thread.Sleep(1000);
        //// Combine the current directory path with the Node.js script name.
        //string textFilePath = System.IO.Path.Combine(twoFoldersUp, "output.txt");
        //// Read the output from the output.txt file.
        //string output = File.ReadAllText(textFilePath);
        var pid = process.Id;
        return pid;
    }

    static void Dispose(Process process)
    {
        process.CloseMainWindow();
        process.WaitForExit();
    }

    // Import required functions from kernel32.dll
    [DllImport("kernel32.dll")]
    private static extern bool FreeConsole();

    [DllImport("kernel32.dll")]
    private static extern bool AttachConsole(int dwProcessId);

    [DllImport("kernel32.dll")]
    private static extern bool GenerateConsoleCtrlEvent(uint dwCtrlEvent, uint dwProcessGroupId);

    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleCtrlHandler(IntPtr handlerRoutine, bool add);

    // Define constants for the Ctrl+C event
    private const uint CTRL_C_EVENT = 0;

    static void SendCtrlC(int pid)
    {

        Console.WriteLine("sending ctrl+c to pid {0}", pid);
        FreeConsole();

        if (AttachConsole(pid))
        {
            // Disable Ctrl-C handling for our own program, so we don't "kill" ourselves on accident
            SetConsoleCtrlHandler(IntPtr.Zero, true);
            GenerateConsoleCtrlEvent(CTRL_C_EVENT, 0);
            // You may choose to wait here for the process to exit or perform other actions.
            // See: http://stackoverflow.com/a/31020562/32453
        }
        else
        {
            LogLastError(); // failure, PID might no longer exist, show a GUI error window
        }
    }

    static void LogLastError()
    {
        Console.WriteLine("failed to attach Console");
    }


}
