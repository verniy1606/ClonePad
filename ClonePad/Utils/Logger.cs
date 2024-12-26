using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace ClonePad.Utils {
    internal class Logger {
        internal static void Info(string message, [CallerMemberName] string callerMethodName = "") {
            Debug.WriteLine($"{callerMethodName}: {message}");
        }
    }
}
