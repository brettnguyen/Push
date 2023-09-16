using Python.Runtime;

namespace Push
{
    public class RunPushUpCounter
    {
        public static void Main()
        {
            //Runtime.PythonDLL = "/opt/homebrew/Cellar/python@3.9/3.9.6/Frameworks/Python.framework/Versions/3.9/lib/libpython3.9.dylib";
            //PythonEngine.PythonPath = $"{Program.ScriptsDir};{py_path}";
            //PythonEngine.PythonHome = "/opt/homebrew/Cellar/python@3.9/3.9.6/Frameworks/Python.framework/Versions/3.9/lib/python3.9";
           // PythonEngine.ProgramName = "PushUpCounter.py";
            PythonEngine.Initialize();
            PythonEngine.BeginAllowThreads();
            using (Py.GIL()) // Acquire the Python GIL (Global Interpreter Lock)
            {
                dynamic PuC = Py.Import("PushUpCounter"); // Import the Python script
                PuC.run(); // Call the function or methods defined in the Python script
            }
        }
    }
}