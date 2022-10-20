package md51b31875105d4bc8f9c803af2f3dc05ab;


public class NumberListAdapterViewHolder
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("qrNumber.NumberListAdapterViewHolder, qrNumber", NumberListAdapterViewHolder.class, __md_methods);
	}


	public NumberListAdapterViewHolder ()
	{
		super ();
		if (getClass () == NumberListAdapterViewHolder.class)
			mono.android.TypeManager.Activate ("qrNumber.NumberListAdapterViewHolder, qrNumber", "", this, new java.lang.Object[] {  });
	}

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
