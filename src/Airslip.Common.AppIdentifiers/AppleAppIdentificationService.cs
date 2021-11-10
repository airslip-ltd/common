using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.AppIdentifiers
{
    public class AppleAppIdentificationService : IAppleAppIdentificationService
    {
        private readonly AppleAppIdentifierSettings _appleAppIdentifierSettings;
        private readonly AndroidAppIdentifierSettings _androidAppIdentifierSettings;

        public AppleAppIdentificationService(
            IOptions<AppleAppIdentifierSettings> appIdentifierOptions,
            IOptions<AndroidAppIdentifierSettings> packageOptions)
        {
            _appleAppIdentifierSettings = appIdentifierOptions.Value;
            _androidAppIdentifierSettings = packageOptions.Value;
        }

        public AppleAppSiteAssociation GetAppSiteAssociation()
        {
            AppleAppIdentifierSetting bankTransactionSettings = _appleAppIdentifierSettings.BankTransactions;
            AppleAppIdentifierSetting identitySettings = _appleAppIdentifierSettings.Identity;

            string bankTransactionPath = BuildDeepLinkingPath(
                bankTransactionSettings.UriSuffix,
                bankTransactionSettings.Version,
                bankTransactionSettings.Endpoint);

            string identityPath = BuildDeepLinkingPath(
                identitySettings.UriSuffix,
                identitySettings.Version,
                identitySettings.Endpoint);

            return new AppleAppSiteAssociation
            {
                Applinks = new Applinks
                {
                    details = new List<Detail>
                    {
                        new()
                        {
                            appIDs = new List<string>
                            {
                                bankTransactionSettings.AppID
                            },
                            components = new List<Component>
                            {
                                new()
                                {
                                   slash = bankTransactionPath,
                                   comment = "Matches any URL whose path starts with " + bankTransactionPath
                                },
                                new()
                                {
                                slash = identityPath,
                                comment = "Matches any URL whose path starts with " + identityPath
                            }
                            }
                        }
                    }
                },
                Webcredentials = new Webcredentials
                {
                    apps = new List<string>
                    {
                        bankTransactionSettings.AppID
                    }
                },
                appclips = new Appclips
                {
                    apps = new List<string>
                    {
                        bankTransactionSettings.AppID
                    }
                }
            };
        }

        public IEnumerable<AssetLink> GetAssetLinks()
        {
            PackageConfiguration androidPackageSettings = _androidAppIdentifierSettings.Android;

            return new List<AssetLink>
            {
                new(androidPackageSettings.Namespace, androidPackageSettings.PackageName,
                    androidPackageSettings.Relation, androidPackageSettings.Sha256CertFingerprints)
            };
        }

        private static string BuildDeepLinkingPath(string uriSuffix, string version, string endpoint)
        {
            return string.IsNullOrWhiteSpace(uriSuffix)
                ? $"/{version}/{endpoint}/*"
                : $"/{uriSuffix}/{version}/{endpoint}/*";
        }
    }
}