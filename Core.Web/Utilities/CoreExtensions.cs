using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Core.Web.Utilities
{
    public enum ReportExportType
    {
        EXCEL,
        PDF,
        WORD
    }
    public static class CoreExtensions
	{		
        //http://stackoverflow.com/questions/13396798/report-viewer-pass-image-from-form-possible
        //Image to String Base64
        public static string ToBase64(this Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        //http://stackoverflow.com/questions/489258/linqs-distinct-on-a-particular-property
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void HideExport(this ReportViewer reportViewer, ReportExportType[] exportTypeList)
        {
            foreach (RenderingExtension extension in reportViewer.LocalReport.ListRenderingExtensions())
            {
                System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    fieldInfo.SetValue(extension, false);              
            } 

            //string[] exportTypeArr = (from a in exportTypeList
            //                          select Enum.GetName(a.GetType(), a)
            //                         )
            //                         .ToArray();            

            //List<RenderingExtension> extensions = reportViewer.LocalReport.ListRenderingExtensions().Where(x => exportTypeArr.Contains(x.Name)).ToList();
            //if (extensions != null && extensions.Count() > 0)
            //{
            //    foreach (var extension in extensions)
            //    {
            //        System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField("m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            //        fieldInfo.SetValue(extension, false);
            //    }                
            //}
        }
       
	}
}