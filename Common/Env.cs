using System;
using System.Collections;

namespace Aspor.Common
{

    //Simplified class
    public static class Env
    {

        //
        // Summary:
        //     Retrieves the value of an environment variable from the current process or from
        //     the Windows operating system registry key for the current user or local machine.
        //
        // Parameters:
        //   variable:
        //     The name of an environment variable.
        //
        //   target:
        //     One of the System.EnvironmentVariableTarget values. Only System.EnvironmentVariableTarget.Process
        //     is supported on .NET Core running on Unix-bases systems.
        //
        // Returns:
        //     The value of the environment variable specified by the variable and target parameters,
        //     or null if the environment variable is not found.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     variable is null.
        //
        //   T:System.ArgumentException:
        //     target is not a valid System.EnvironmentVariableTarget value.
        //
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission to perform this operation.
        public static string? Get(string variable, EnvironmentVariableTarget target)
        {
            return Environment.GetEnvironmentVariable(variable, target);
        }
        //
        // Summary:
        //     Retrieves the value of an environment variable from the current process.
        //
        // Parameters:
        //   variable:
        //     The name of the environment variable.
        //
        // Returns:
        //     The value of the environment variable specified by variable, or null if the environment
        //     variable is not found.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     variable is null.
        //
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission to perform this operation.
        public static string? GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }
        //
        // Summary:
        //     Retrieves all environment variable names and their values from the current process,
        //     or from the Windows operating system registry key for the current user or local
        //     machine.
        //
        // Parameters:
        //   target:
        //     One of the System.EnvironmentVariableTarget values. Only System.EnvironmentVariableTarget.Process
        //     is supported on .NET Core running on Unix-based systems.
        //
        // Returns:
        //     A dictionary that contains all environment variable names and their values from
        //     the source specified by the target parameter; otherwise, an empty dictionary
        //     if no environment variables are found.
        //
        // Exceptions:
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission to perform this operation for
        //     the specified value of target.
        //
        //   T:System.ArgumentException:
        //     target contains an illegal value.
        public static IDictionary Get(EnvironmentVariableTarget target)
        {
            return Environment.GetEnvironmentVariables(target);
        }
        //
        // Summary:
        //     Retrieves all environment variable names and their values from the current process.
        //
        // Returns:
        //     A dictionary that contains all environment variable names and their values; otherwise,
        //     an empty dictionary if no environment variables are found.
        //
        // Exceptions:
        //   T:System.Security.SecurityException:
        //     The caller does not have the required permission to perform this operation.
        //
        //   T:System.OutOfMemoryException:
        //     The buffer is out of memory.
        public static IDictionary Get()
        {
            return Environment.GetEnvironmentVariables();
        }

    }
}
