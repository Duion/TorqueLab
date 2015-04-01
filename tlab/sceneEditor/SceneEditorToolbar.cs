//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function Lab::toggleObjectCenter( %this,%updateOnly ) {
   if (!%updateOnly)
      EWorldEditor.objectsUseBoxCenter = !EWorldEditor.objectsUseBoxCenter;
      
	if( EWorldEditor.objectsUseBoxCenter ) {
	        SceneEditorToolbar-->centerObject.setBitmap("tlab/gui/icons/toolbar_assets/SelObjectCenter.png");
	    } else {
	         SceneEditorToolbar-->centerObject.setBitmap("tlab/gui/icons/toolbar_assets/SelObjectBounds.png");
	    }

}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::toggleObjectTransform( %this ) {
    if (!%updateOnly){
         if( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" ) 
              GlobalGizmoProfile.setFieldValue(alignment, World);
         else
              GlobalGizmoProfile.setFieldValue(alignment, Object);
    }
    
   if( GlobalGizmoProfile.getFieldValue(alignment) $= "Object" ) {
      SceneEditorToolbar-->objectTransform.setBitmap("tlab/gui/icons/toolbar_assets/TransformObject.png");
   } else {
      SceneEditorToolbar-->objectTransform.setBitmap("tlab/gui/icons/toolbar_assets/TransformWorld");
   }
	
}
//------------------------------------------------------------------------------

//==============================================================================
function Lab::setSelectionLock( %this,%locked ) {
   EWorldEditor.lockSelection(%locked); 
   EWorldEditor.syncGui();
	
}
//------------------------------------------------------------------------------
//==============================================================================
function Lab::setSelectionVisible( %this,%visible ) {
  EWorldEditor.hideSelection(!%visible); 
  EWorldEditor.syncGui();
	
}
//------------------------------------------------------------------------------
