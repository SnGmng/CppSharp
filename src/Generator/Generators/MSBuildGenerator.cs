using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CppSharp.AST;

namespace CppSharp.Generators
{
    public class MSBuildGenerator : CodeGenerator
    {
        public MSBuildGenerator(Module module, Dictionary<Module, string> libraryMappings, string outputDir)
            : base(null)
        {
            this.module = module;
            this.libraryMappings = libraryMappings;
            this.outputDir = outputDir;
        }

        public override string FileExtension => "csproj";

        public override void Process()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            WriteLine($@"
<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFramework>netstandard2.1</TargetFramework>
    <PlatformTarget>AnyCPU</PlatformTarget>
    <OutputPath>{outputDir}</OutputPath>
    <DocumentationFile>{module.LibraryName}.xml</DocumentationFile>
    <Configuration>Release</Configuration>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <EnableDefaultNoneItems>false</EnableDefaultNoneItems>
    <EnableDefaultItems>false</EnableDefaultItems>
    <AppendTargetFrameworkToOutputPath>false</AppendTargetFrameworkToOutputPath>
  </PropertyGroup>
  <ItemGroup>
    {string.Join(Environment.NewLine,
         module.CodeFiles.Select(c => $"<Compile Include=\"{c}\" />"))}
  </ItemGroup>
  <ItemGroup>
    {string.Join(Environment.NewLine,
         new[] { Path.Combine(Path.GetDirectoryName(location), "CppSharp.Runtime.dll") }
         .Union(from dependency in module.Dependencies
                where libraryMappings.ContainsKey(dependency)
                select libraryMappings[dependency])
         .Select(reference =>
 $@"<Reference Include=""{Path.GetFileNameWithoutExtension(reference)}"">
      <HintPath>{reference}</HintPath>
    </Reference>"))}
  </ItemGroup>
</Project>".Trim());
        }

        private readonly Module module;
        private readonly Dictionary<Module, string> libraryMappings;
        private readonly string outputDir;
    }
}
