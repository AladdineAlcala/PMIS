using System.Web;
using System.Web.Optimization;

namespace PMIS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/javascript").Include(
                "~/Content/plugins/jquery/jquery.min.js",
                "~/Content/plugins/bootstrap/js/bootstrap.bundle.min.js",
                "~/Content/plugins/moment/moment.min.js",
                "~/Content/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",
                "~/Content/plugins/fastclick/fastclick.js",
                "~/Content/plugins/datatables/jquery.dataTables.min.js",
                "~/Content/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js",
                //"~/Content/plugins/datatables-responsive/js/dataTables.responsive.min.js",
                "~/Content/plugins/datatables-buttons/js/dataTables.buttons.min.js",
                "~/Content/plugins/datatables-select/js/select.bootstrap4.min.js",
                "~/Content/plugins/typeahead/js/typeahead.bundle.min.js",
                "~/Content/plugins/sweetalert2/sweetalert2.min.js",
                "~/Content/plugins/moment/datetime-moment.js",
                "~/Content/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",
                "~/Content/plugins/toastr/toastr.min.js",
                //"~/Content/plugins/summernote/summernote-bs4.min.js",
                "~/Content/plugins/webcam/webcam.min.js",
                "~/Content/dist/js/adminlte.min.js"

                ));

            bundles.Add(new ScriptBundle("~/bundles/AjaxExtensions").Include(
                "~/Content/ajaxextensions/jquery.unobtrusive-ajax.js",
                "~/Content/ajaxextensions/jquery-validate.js",
                "~/Content/ajaxextensions/jquery.validate.unobtrusive.js"
                ));


            bundles.Add(new ScriptBundle("~/scriptbundles/fullcalendarjs").Include(
                "~/Content/plugins/fullcalendar/main.min.js",
                "~/Content/plugins/fullcalendar-bootstrap/main.min.js",
                "~/Content/plugins/fullcalendar-daygrid/main.min.js"
                ));

            //bundles.Add(new ScriptBundle("~/scripts/DocsLayout").Include(
            //    "~/Content/plugins/jquery/jquery.min.js",
            //    "~/Content/plugins/bootstrap/js/bootstrap.bundle.min.js",
            //    "~/Content/plugins/sweetalert2/sweetalert2.min.js",
            //    "~/Content/plugins/summernote/summernote-bs4.min.js",
            //    "~/Content/dist/js/adminlte.min.js"
            //    ));


            bundles.Add(new StyleBundle("~/Styles/Css").Include(
                 "~/Content/plugins/fontawesome-pro-5.11.2-web/css/all.min.css",
                 "~/Content/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                 "~/Content/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                 "~/Content/plugins/datatables-bs4/css/dataTables.bootstrap4.min.css",
                 //"~/Content/plugins/datatables-responsive/css/responsive.bootstrap4.min.css",
                 "~/Content/plugins/datatables-buttons/css/buttons.bootstrap4.min.css",
                 "~/Content/plugins/datatables-select/css/select.bootstrap4.min.css",
                 "~/Content/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css",
                 "~/Content/plugins/toastr/toastr.min.css",
                 "~/Content/plugins/typeahead/css/typehead.css",
                 //"~/Content/plugins/summernote/summernote-bs4.css",
                 "~/Content/PagedList.css"
                ));

            bundles.Add(new StyleBundle("~/adminstyle").Include(
                "~/Content/dist/css/adminlte.min.css",
                "~/Content/customcss/pmis_css.css"
            ));

            bundles.Add(new StyleBundle("~/stylebundles/fullcalendarcss").Include(
                 "~/Content/plugins/fullcalendar/main.min.css",
                 "~/Content/plugins/fullcalendar-bootstrap/main.min.css",
                 "~/Content/plugins/fullcalendar-daygrid/main.min.css"

            ));


            bundles.Add(new StyleBundle("~/Styles/Docs").Include(
                "~/Content/plugins/fontawesome-pro-5.11.2-web/css/all.min.css",
                "~/Content/plugins/sweetalert2-theme-bootstrap-4/bootstrap-4.min.css",
                "~/Content/plugins/summernote/summernote-bs4.css",
                "~/Content/dist/css/adminlte.min.css",
                "~/Content/customcss/pmis_css.css"
                  ));
        }
    }
}
