using System.Linq;
using Cake.Common.IO;
using Cake.Common.Solution;
using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.NuGet;
using Cake.Core;
using Cake.Core.Diagnostics;
using System;
using System.IO;
using System.Collections.Generic;

namespace CodeCake
{
    /// <summary>
    /// Sample build "script".
    /// It can be decorated with AddPath attributes that inject paths into the PATH environment variable. 
    /// </summary>
    [AddPath( "CodeCakeBuilder/Tools" )]
    public class Build : CodeCakeHost
    {
        public Build()
        {

            Task( "Clean" )
                .Does( () =>
                {
                    Directory.Delete( Path.Combine( Cake.Environment.WorkingDirectory.FullPath, "Galt\\bin\\" ), true );
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
                    var listProject = Directory.GetDirectories(Path.GetFullPath("../../../../../../src"));

                    foreach( string proj in listProject )
                    {
                        string[] projsplitted = proj.Split( '\\' );
                        if( projsplitted[projsplitted.Length - 1] != "CodeCakeBuilder" )
                        {
                            Cake.DotNetCoreBuild( proj );
                        }
                    }
                } );
            Task( "Tests" )
                .IsDependentOn( "Build" )
                .Does( () =>
                 {
                     //Cake.DotNetCoreExecute()
                 } );

            // The Default task for this script can be set here.
            Task( "Default" )
                .IsDependentOn( "Clean" );
        }

        public string[] GetListProject()
        {
            string[] listProject = Directory.GetDirectories("./");

            foreach( string proj in listProject )
            {
                string[] filtredListProject = proj.Split( '\\' );
                if( filtredListProject[filtredListProject.Length - 1] != "CodeCakeBuilder" )
                {
                    Cake.DotNetCoreBuild( proj );
                }
            }

            return null;
        }
    }
}