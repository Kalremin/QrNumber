package md51b31875105d4bc8f9c803af2f3dc05ab;


public class PopupNameActivity
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_okclick:(Landroid/view/View;)V:__export__\n" +
			"n_onTouchEvent:(Landroid/view/MotionEvent;)Z:GetOnTouchEvent_Landroid_view_MotionEvent_Handler\n" +
			"";
		mono.android.Runtime.register ("qrNumber.PopupNameActivity, qrNumber", PopupNameActivity.class, __md_methods);
	}


	public PopupNameActivity ()
	{
		super ();
		if (getClass () == PopupNameActivity.class)
			mono.android.TypeManager.Activate ("qrNumber.PopupNameActivity, qrNumber", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void okclick (android.view.View p0)
	{
		n_okclick (p0);
	}

	private native void n_okclick (android.view.View p0);


	public boolean onTouchEvent (android.view.MotionEvent p0)
	{
		return n_onTouchEvent (p0);
	}

	private native boolean n_onTouchEvent (android.view.MotionEvent p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
