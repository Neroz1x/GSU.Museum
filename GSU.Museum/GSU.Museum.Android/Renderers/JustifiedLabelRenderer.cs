using Android.Content;
using GSU.Museum.Droid.Renderers;
using GSU.Museum.Shared.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(JustifiedLabel), typeof(JustifiedLabelRenderer))]
namespace GSU.Museum.Droid.Renderers
{
    public class JustifiedLabelRenderer : LabelRenderer
    {
        public JustifiedLabelRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.JustificationMode = Android.Text.JustificationMode.InterWord;
            }
        }
    }
}