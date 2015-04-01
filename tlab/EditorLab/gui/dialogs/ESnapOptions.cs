//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$ESnapOptions_Initialized = false;
//==============================================================================
function ESnapOptions::ToggleVisibility(%this) {
	if ( %this.visible  ) {
		%this.setVisible(false);
		SnapToBar-->snappingSettingsBtn.setStateOn(false);
	} else {
		%this.setVisible(true);
		%this.selectWindow();
		%this.setCollapseGroup(false);
		%this.onShow();
		SnapToBar-->snappingSettingsBtn.setStateOn(true);
	}
}
//------------------------------------------------------------------------------

function ESnapOptions::onShow( %this ) {
	if(!$ESnapOptions_Initialized) {
		%this.position = %this-->snappingSettingsBtn.position;
		$ESnapOptions_Initialized = true;
	}
	%this-->TabPage_Terrain-->NoAlignment.setStateOn(1);

	%this-->TabPage_Soft-->NoAlignment.setStateOn(1);
	%this-->TabPage_Soft-->RenderSnapBounds.setStateOn(1);
	%this-->TabPage_Soft-->SnapBackfaceTolerance.setText(EWorldEditor.getSoftSnapBackfaceTolerance());
}

function ESnapOptions::hideDialog( %this ) {
	%this.setVisible(false);
	SnapToBar-->snappingSettingsBtn.setStateOn(false);
}

function ESnapOptions::ToggleVisibility() {
	if ( ESnapOptions.visible  ) {
		ESnapOptions.setVisible(false);
		SnapToBar-->snappingSettingsBtn.setStateOn(false);
	} else {
		ESnapOptions.setVisible(true);
		ESnapOptions.selectWindow();
		ESnapOptions.setCollapseGroup(false);
		SnapToBar-->snappingSettingsBtn.setStateOn(true);
	}
}

function ESnapOptions::setTerrainSnapAlignment( %this, %val ) {
	EWorldEditor.setTerrainSnapAlignment(%val);
}

function ESnapOptions::setSoftSnapAlignment( %this, %val ) {
	EWorldEditor.setSoftSnapAlignment(%val);
}

function ESnapOptions::setSoftSnapSize( %this ) {
	%val = ESnapOptions-->SnapSize.getText();

	EWorldEditor.setSoftSnapSize(%val);
	if (EditorIsActive())
	   EWorldEditor.syncGui();
}

function ESnapOptions::setGridSnapSize( %this ) {
	%val = ESnapOptions-->GridSize.getText();

	EWorldEditor.setGridSize( %val );
}

function ESnapOptions::toggleRenderSnapBounds( %this ) {
	EWorldEditor.softSnapRender( ESnapOptionsTabSoft-->RenderSnapBounds.getValue() );
}

function ESnapOptions::toggleRenderSnappedTriangle( %this ) {
	EWorldEditor.softSnapRenderTriangle( ESnapOptionsTabSoft-->RenderSnappedTriangle.getValue() );
}

function ESnapOptions::getSoftSnapBackfaceTolerance( %this ) {
	%val = ESnapOptions-->SnapBackfaceTolerance.getText();

	EWorldEditor.setSoftSnapBackfaceTolerance(%val);
}