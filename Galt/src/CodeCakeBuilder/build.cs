using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Core;
using Cake.Core.IO;
using Cake.Common.Tools.OpenCover;
using Cake.Common.Tools.NUnit;

namespace CodeCake
{
    /// <summary>
    /// Sample build "script".
    /// It can be decorated with AddPath attributes that inject paths into the PATH environment variable. 
    /// </summary>
    public class Build : CodeCakeHost
    {
        public Build()
        {

            Task( "Clean" )
                .Does( () =>
                {
                    DirectoryPathCollection AllProj = Cake.GetDirectories( "./*", p => !p.Path.FullPath.Contains("CodeCakeBuilder" ));
                    foreach( DirectoryPath proj in AllProj )
                    {
                        if( Cake.DirectoryExists( proj + "/bin" ) )
                        {
                            Cake.DeleteDirectory( proj + "/bin", true );
                        }
                        if( Cake.DirectoryExists( proj + "/obj" ) )
                        {
                            Cake.DeleteDirectory( proj + "/obj", true );
                        }
                    }
                } );

            Task( "Restore" )
                .IsDependentOn( "Clean" )
                .Does( () =>
                {
                    Cake.DotNetCoreRestore();
                } );

            Task( "Build" )
                .IsDependentOn( "Restore" )
                .Does( () =>
                {
                    DirectoryPathCollection AllProj = Cake.GetDirectories( "./*", p => !p.Path.FullPath.Contains("CodeCakeBuilder" ));
                    foreach( DirectoryPath proj in AllProj )
                    {
                        Cake.DotNetCoreBuild( proj.FullPath );
                    }
                } );

            Task( "Unit-Tests" )
                .IsDependentOn( "Build" )
                .Does( () =>
                {
                    Cake.OpenCover( tool =>
                    {
                        tool.NUnit3( "./**/*.Tests/**/net46/*/**/*.Tests.exe", new NUnit3Settings
                        {
                            ToolPath = @"C:\Users\Plop\git\Project-Galt-Test\Galt\packages\NUnit.ConsoleRunner\3.5.0\tools\nunit3-console.exe"
                        } );
                    },
                     new FilePath( "./result.xml" ),
                     new OpenCoverSettings
                     {
                         ToolPath = "C:/Users/Plop/git/Project-Galt-Test/Galt/packages/OpenCover/4.6.519/tools/OpenCover.Console.exe",
                         Register = "User"
                     }
                        .WithFilter( "+[Test]*" )
                        .WithFilter( "-[Test.Tests]*" )
                    );
                } );

            // The Default task for this script can be set here.
            Task( "Default" )
                .IsDependentOn( "Unit-Tests" );
        }
    }
}