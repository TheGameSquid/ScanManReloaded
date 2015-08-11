using System;
using System.Windows;
using System.Windows.Media;

namespace ScanManReloaded
{
    public static class Extensions
    {
        public static T FindParent<T>(this DependencyObject obj) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(obj);

            // We've reached the end of the tree
            if (parentObject == null) return null;

            // Check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                // If so: return
                return parent as T;
            }
            else
            {   
                // Else: continue searching
                return FindParent<T>(parentObject);
            }         
        }
    }
}
