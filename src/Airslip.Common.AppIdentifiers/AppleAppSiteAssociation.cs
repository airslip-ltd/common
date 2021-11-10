using System.Collections.Generic;

namespace Airslip.Common.AppIdentifiers
{
    public class AppleAppSiteAssociation
    {
        public AppleAppSiteAssociation(
            string appId,
            params string [] paths)
        {
            Applinks = new Applinks(appId, paths);
        }

        public Applinks Applinks { get; }
    }
    
    public class Applinks
    {
        public Applinks(string appId, string[] paths)
        {
            Details = new List<Detail>()
            {
                new()
                {
                    AppID = appId,
                    Paths = paths
                }
            };
        }

        public IEnumerable<string> Apps { get; set; } = new List<string>(0);
        public IEnumerable<Detail> Details { get; }
    }
    
    public class Detail
    {
        public string? AppID { get; set; }
        public IEnumerable<string>? Paths { get; set; }
    }

    public class Target
    {
        public Target(string @namespace, string packageName, IEnumerable<string> sha256CertFingerprints)
        {
            Namespace = @namespace;
            PackageName = packageName;
            Sha256CertFingerprints = sha256CertFingerprints;
        }

        public string Namespace { get; }
        public string PackageName { get; }
        public IEnumerable<string> Sha256CertFingerprints { get; }
    }
    
    public class AssetLink
    {
        public IEnumerable<string> Relation { get; }
        public Target Target { get; }
        
        public AssetLink(string @namespace, string packageName, IEnumerable<string> relation, IEnumerable<string> sha256CertFingerprints)
        {
            Relation = relation;
            Target = new Target(@namespace, packageName, sha256CertFingerprints);
        }
    }
}