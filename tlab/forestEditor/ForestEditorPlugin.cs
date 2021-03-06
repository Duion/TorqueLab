//==============================================================================
// LabEditor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//==============================================================================
// Forest Editor Params - Used set default settings and build plugins options GUI
//==============================================================================
function ForestEditorPlugin::initParamsArray( %this,%cfgArray ) {
	$ForestEditorCfg = newScriptObject("ForestEditorCfg");
	%cfgArray.group[%groupId++] = "General settings";
	%cfgArray.setVal("DefaultBrush",    "BaseBrush" TAB "DefaultBrush" TAB "TextEdit" TAB "" TAB "ForestEditorPlugin" TAB %groupId);
	%cfgArray.setVal("BrushPressure",       "2" TAB "Brush pressure" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("BrushSize",       "5" TAB "BrushSize" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("BrushHardness",       "2" TAB "BrushHardness" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("GlobalScale",       "2" TAB "GlobalScale" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.group[%groupId++] = "Default brush settings";
	%cfgArray.setVal("DefaultBrushPressure",    "20" TAB "Default brush pressure" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("DefaultBrushHardness",    "50" TAB "Default brush gardness" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("DefaultBrushSize",    "5" TAB "Default brush size" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
	%cfgArray.setVal("DefaultGlobalScale",    "1" TAB "Default global scale pressure" TAB "TextEdit" TAB "" TAB "ForestEditorCfg" TAB %groupId);
}
//==============================================================================
// Plugin Object Callbacks - Called from TLab plugin management scripts
//==============================================================================
//==============================================================================
// Called when TorqueLab is launched for first time
function ForestEditorPlugin::onWorldEditorStartup( %this ) {
	Parent::onWorldEditorStartup( %this );
	new PersistenceManager( ForestDataManager );
	%brushPath = "art/forest/brushes.cs";

	if ( !isFile( %brushPath ) )
		createPath( %brushPath );

	// This creates the ForestBrushGroup, all brushes, and elements.
	exec( %brushpath );

	if ( !isObject( ForestBrushGroup ) ) {
		new SimGroup( ForestBrushGroup );
		ForestBrushGroup.internalName = "ForestBrush";
		%this.showError = true;
	}

	ForestEditBrushTree.open( ForestBrushGroup );

	if ( !isObject( ForestItemDataSet ) )
		new SimSet( ForestItemDataSet );

	if ( !isObject( ForestMeshGroup ) )
		new SimGroup( ForestMeshGroup );

	ForestEditMeshTree.open( ForestItemDataSet );
	ForestEditTabBook.selectPage(0);
	ForestEditToolbar-->globalScaleEdit.setValue("1");
	//FEP_ForestManager.init();
	ForestEditorPlugin.brushPressure = ForestEditorCfg.defaultBrushPressure;
	FEP_BrushManager.setBrushPressure();
}

function ForestEditorPlugin::onWorldEditorShutdown( %this ) {
	if ( isObject( ForestBrushGroup ) )
		ForestBrushGroup.delete();

	if ( isObject( ForestDataManager ) )
		ForestDataManager.delete();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is activated (Active TorqueLab plugin)
function ForestEditorPlugin::onActivated( %this ) {
	EditorGui.bringToFront( ForestEditorGui );
	ForestEditorGui.setVisible( true );
	ForestEditorGui.makeFirstResponder( true );
	//ForestEditToolbar.setVisible( true );
	%this.map.push();
	Parent::onActivated(%this);
	ForestEditBrushTree.open( ForestBrushGroup );
	ForestEditMeshTree.open( ForestItemDataSet );
	// Open the Brush tab.
	ForestEditTabBook.selectPage(0);
	// Sync the pallete button state
	%forestBrushSize = %this.getCfg("BrushSize");
	%this.previousBrushSize = ETerrainEditor.getBrushSize();
	ETerrainEditor.setBrushSize(%forestBrushSize);
	// And toolbar.
	%tool = ForestEditorGui.getActiveTool();

	if ( isObject( %tool ) )
		%tool.onActivated();

	if ( !isObject( %tool ) ) {
		ForestEditorPaintModeBtn.performClick();

		if ( ForestEditBrushTree.getItemCount() > 0 ) {
			ForestEditBrushTree.selectItem( 0, true );
		}
	} else if ( %tool == ForestTools->SelectionTool ) {
		%mode = GlobalGizmoProfile.mode;

		switch$ (%mode) {
		case "None":
			ForestEditorSelectModeBtn.performClick();

		case "Move":
			ForestEditorMoveModeBtn.performClick();

		case "Rotate":
			ForestEditorRotateModeBtn.performClick();

		case "Scale":
			ForestEditorScaleModeBtn.performClick();
		}
	} else if ( %tool == ForestTools->BrushTool ) {
		%mode = ForestTools->BrushTool.mode;

		switch$ (%mode) {
		case "Paint":
			ForestEditorPaintModeBtn.performClick();

		case "Erase":
			ForestEditorEraseModeBtn.performClick();

		case "EraseSelected":
			ForestEditorEraseSelectedModeBtn.performClick();
		}
	}

	if ( %this.showError )
		LabMsgOK( "Error", "Your art/forest folder does not contain a valid brushes.cs. Brushes you create will not be saved!" );

	//Check if forest brushes settings are set, if not set defaults
	if (  ForestEditorPlugin.brushPressure $= "")
		ForestEditorPlugin.brushPressure = ForestEditorCfg.defaultBrushPressure;

	if (  ForestEditorPlugin.brushHardness $= "")
		ForestEditorPlugin.brushHardness = ForestEditorCfg.defaultBrushHardness;

	if (  ForestEditorPlugin.brushSize $= "")
		ForestEditorPlugin.brushSize = ForestEditorCfg.defaultBrushSize;

	if (  ForestEditorPlugin.globalScale $= "")
		ForestEditorPlugin.globalScale = ForestEditorCfg.defaultGlobalScale;

	//Set the forest brush settings
	FEP_BrushManager.setBrushPressure();
	FEP_BrushManager.setBrushHardness();
	FEP_BrushManager.setBrushSize();
	FEP_BrushManager.setGlobalScale();
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when the Plugin is deactivated (active to inactive transition)
function ForestEditorPlugin::onDeactivated( %this ) {
	ForestEditorGui.setVisible( false );
	ETerrainEditor.setBrushSize( this.previousBrushSize);
	%tool = ForestEditorGui.getActiveTool();

	if ( isObject( %tool ) )
		%tool.onDeactivated();

	// Also take this opportunity to save.
	ForestDataManager.saveDirty();
	%this.map.pop();
	Parent::onDeactivated(%this);
}
//------------------------------------------------------------------------------
//==============================================================================
// Called from TorqueLab after plugin is initialize to set needed settings
function ForestEditorPlugin::onPluginCreated( %this ) {
	EWorldEditor.dropType = SceneEditorPlugin.getCfg("DropType");
}
//------------------------------------------------------------------------------

//==============================================================================
// Called when the mission file has been saved
function SceneEditorPlugin::onSaveMission( %this, %file ) {
}
//------------------------------------------------------------------------------
//==============================================================================
// Called when TorqueLab is closed
function ForestEditorPlugin::onEditorSleep( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
// Callbacks Handlers - Called on specific editor actions
//==============================================================================

//==============================================================================
//
function SceneEditorPlugin::handleDelete( %this ) {
	// The tree handles deletion and notifies the
	// world editor to clear its selection.
	//
	// This is because non-SceneObject elements like
	// SimGroups also need to be destroyed.
	//
	// See EditorTree::onObjectDeleteCompleted().
	%selSize = EWorldEditor.getSelectionSize();

	if( %selSize > 0 )
		SceneEditorTree.deleteSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::handleDeselect() {
	EWorldEditor.clearSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::handleCut() {
	EWorldEditor.cutSelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::handleCopy() {
	EWorldEditor.copySelection();
}
//------------------------------------------------------------------------------
//==============================================================================
function SceneEditorPlugin::handlePaste() {
	EWorldEditor.pasteSelection();
}
//------------------------------------------------------------------------------