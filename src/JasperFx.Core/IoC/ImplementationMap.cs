﻿using JasperFx.Core.Reflection;
using JasperFx.Core.TypeScanning;
using Microsoft.Extensions.DependencyInjection;

namespace JasperFx.Core.IoC;

internal class ImplementationMap : IRegistrationConvention
{
    private readonly ServiceLifetime _lifetime;

    public ImplementationMap(ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        _lifetime = lifetime;
    }

    public void ScanTypes(TypeSet types, IServiceCollection services)
    {
        var interfaces = types.FindTypes(TypeClassification.Interfaces | TypeClassification.Closed)
            .Where(x => x != typeof(IDisposable));
        var concretes = types.FindTypes(TypeClassification.Concretes | TypeClassification.Closed)
            .Where(x => x.GetConstructors().Any()).ToArray();

        interfaces.Each(@interface =>
        {
            var implementors = concretes.Where(x => x.CanBeCastTo(@interface)).ToArray();
            if (implementors.Count() == 1)
            {
                services.Add(new ServiceDescriptor(@interface, implementors.Single(), _lifetime));
            }
        });
    }

    public override string ToString()
    {
        return "Register any single implementation of any interface against that interface";
    }
}