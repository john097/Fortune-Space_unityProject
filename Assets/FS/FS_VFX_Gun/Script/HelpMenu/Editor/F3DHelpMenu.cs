using UnityEngine;
using UnityEditor;

namespace Forge3D
{
    public class F3DHelpMenu
    {
        #region Variables
        const int priority = 1100;

        static string AssetStoreURL = "http://store.forge3d.com/";
        static string WebURL = "http://forge3d.com";
        static string TwitterURL = "https://twitter.com/Forge_3D";
        static string FacebookURL = "http://facebook.com/forge3d";
        static string SupportURL = "http://forge3d.com/contact/";
        static string DocumentationURL = "http://forge3d.com/documentation/";
        static string BlogURL = "http://forge3d.com/blog/";
        static string ForumURL = "http://forum.forge3d.com/";
        static string BugReportURL = "http://forum.forge3d.com/c/bug-report";
        static string KnownIssuesURL = "http://forum.forge3d.com/c/known-issues";


        #endregion

        #region Menu Items
        [MenuItem("FORGE3D/Community/Asset Store", false, priority + 20)]
        static void AssetStore()
        {
            Application.OpenURL(AssetStoreURL);
        }

        [MenuItem("FORGE3D/Community/Website", false, priority * 2 + 21)]
        static void Web()
        {
            Application.OpenURL(WebURL);
        }

        [MenuItem("FORGE3D/Community/Blog", false, priority * 2 + 22)]
        static void Blog()
        {
            Application.OpenURL(BlogURL);
        }

        [MenuItem("FORGE3D/Community/Forum", false, priority * 2 + 23)]
        static void Forum()
        {
            Application.OpenURL(ForumURL);
        }

        [MenuItem("FORGE3D/Community/Twitter", false, priority * 3 + 24)]
        static void Twitter()
        {
            Application.OpenURL(TwitterURL);
        }

        [MenuItem("FORGE3D/Community/Facebook", false, priority * 3 + 25)]
        static void Facebook()
        {
            Application.OpenURL(FacebookURL);
        }
        
    
        [MenuItem("FORGE3D/Help/Documentation", false, priority * 4 + 26)]
        static void Documentation()
        {
            Application.OpenURL(DocumentationURL);
        }

        [MenuItem("FORGE3D/Help/Known Issues", false, priority * 5 + 27)]
        static void KnownIssues()
        {
            Application.OpenURL(KnownIssuesURL);
        }

        [MenuItem("FORGE3D/Help/Report a Bug", false, priority * 5 + 28)]
        static void BugReport()
        {
            Application.OpenURL(BugReportURL);
        }

        [MenuItem("FORGE3D/Help/Contact Support", false, priority * 6 + 29)]
        static void Support()
        {
            Application.OpenURL(SupportURL);
        }



        #endregion
    }
}