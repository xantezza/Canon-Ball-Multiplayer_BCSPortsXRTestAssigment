﻿using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Providers.DefaultConfigProvider;
using UnityEngine;
using Zenject;

namespace Infrastructure.Installers.ProjectContext
{
    public class ProviderInstallers : MonoInstaller
    {
        
        [SerializeField] private AssetReferenceProvider _assetReferenceProvider;
        [SerializeField] private CachedDefaultConfigProvider cachedDefaultConfigProvider;

        public override void InstallBindings()
        {
            BindAssetReferenceProvider();
            BindDefaultConfigProvider();
        }

        private void BindAssetReferenceProvider() => Container.Bind<AssetReferenceProvider>().FromInstance(_assetReferenceProvider).AsSingle().NonLazy();
        private void BindDefaultConfigProvider() => Container.BindInterfacesTo<CachedDefaultConfigProvider>().FromInstance(cachedDefaultConfigProvider).AsSingle().NonLazy();
    }
}