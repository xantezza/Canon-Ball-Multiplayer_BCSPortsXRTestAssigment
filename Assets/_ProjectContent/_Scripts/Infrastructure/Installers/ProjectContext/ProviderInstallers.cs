using Infrastructure.Providers.AssetReferenceProvider;
using Infrastructure.Providers.DefaultConfigProvider;
using Infrastructure.Providers.PlayerDataProvider;
using Org.BouncyCastle.Asn1.Esf;
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
            BindPlayerDataProvider();
        }

        private void BindAssetReferenceProvider()
        {
            Container.BindInterfacesTo<AssetReferenceProvider>().FromInstance(_assetReferenceProvider).AsSingle().NonLazy();
        }

        private void BindDefaultConfigProvider()
        {
            Container.BindInterfacesTo<CachedDefaultConfigProvider>().FromInstance(cachedDefaultConfigProvider).AsSingle().NonLazy();
        }

        private void BindPlayerDataProvider()
        {
            Container.BindInterfacesTo<PlayerDataProvider>().FromNew().AsSingle().NonLazy();
        }
    }
}