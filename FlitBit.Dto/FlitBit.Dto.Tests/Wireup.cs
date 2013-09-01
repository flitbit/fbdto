using FlitBit.Wireup.Meta;

// Any assembly dependent on DTO will need this declaration:
[assembly: WireupDependency(typeof(FlitBit.Dto.AssemblyWireup))]
// This declaration signifies this assembly is dependent on IoC being wired.
[assembly: WireupDependency(typeof(FlitBit.IoC.AssemblyWireup))]