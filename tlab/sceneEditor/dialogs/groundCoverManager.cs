//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
$GroundCover::Default_["Material"] = "fpMeadowA01_baseFarmGrass_MeadowA30";
$GroundCover::Default_["radius"] = "400";
$GroundCover::Default_["dissolveRadius"] = "200";
$GroundCover::Default_["shapeCullRadius"] = "400";
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::onWake( %this ) {
	hide(SEP_GroundCoverInspectorScroll);
	hide(SEP_GroundCoverLayerPill);
	SEP_GroundCoverSaveButton.active = SEP_GroundCover.isDirty;
	SEP_GroundCover.getMissionGroundCover();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::onSleep( %this ) {
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::createGroundCover( %this ) {
	%name = getUniqueName("envGroundCover");
	%groundCover = new GroundCover(%name) {
		Material = $GroundCover::Default_["Material"];
		radius = $GroundCover::Default_["radius"];
		dissolveRadius = $GroundCover::Default_["dissolveRadius"];
		shapeCullRadius = $GroundCover::Default_["shapeCullRadius"];
	};
	%group = SceneCreatorWindow.getActiveSimGroup();
	%group.add(%groundCover);
	SEP_GroundCover.selectedGroundCover = %groundCover;
	%this.getMissionGroundCover();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::toggleInspector( %this ) {
	SEP_GroundCoverInspectorScroll.visible = !SEP_GroundCoverInspectorScroll.visible;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::updateFieldValue( %this,%field,%value,%layerId ) {
	%obj = SEP_GroundCover.selectedGroundCover;

	if (!isObject(%obj)) {
		warnLog("Can't update ground cover value because none is selected. Tried wth:",%obj);
		return;
	}

	%currentValue = %obj.getFieldValue(%field,%layerId);

	if (%currentValue $= %value) {		
		return;
	}

	GroundCoverInspector.apply();
	//eval("%obj."@%checkField@" = %value;");
	%obj.setFieldValue(%field,%value,%layerId);
	EWorldEditor.isDirty = true;
	%this.setDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::setDirty( %this ) {
	SEP_GroundCover.isDirty = true;
	SEP_GroundCoverSaveButton.active = true;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::setNotDirty( %this ) {
	SEP_GroundCover.isDirty = false;
	SEP_GroundCoverSaveButton.active = false;
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::saveData( %this ) {
	%obj = SEP_GroundCover.selectedGroundCover;

	if (!isObject(%obj)) {
		warnLog("Can't update ground cover value because none is selected. Tried wth:",%obj);
		return;
	}

	Lab_PM.setDirty(%obj);
	Lab_PM.saveDirty();
	%this.setNotDirty();
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::deleteObj( %this ) {
	%obj = SEP_GroundCover.selectedGroundCover;

	LabMsgOkCancel("Delete selected GroundCover","You are about to delete the current GroundCover:" SPC %obj.getName() @ ". Proceed with GroundCover deletion?","delObj("@%obj.getId()@");");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::rebuildSettings( %this ) {
	SEP_GroundCover.buildLayerSettingGui();
}
//------------------------------------------------------------------------------

//==============================================================================
// SEP_GroundCover.getMissionGroundCover();
function SEP_GroundCover::getMissionGroundCover( %this ) {	

	SEP_GroundCover.groundCoverList = Lab.getMissionObjectClassList("GroundCover");
	SEP_GroundCoverMenu.clear();
	SEP_GroundCoverMenu.add("None selected",0);
	%selected = 0;
	
		
	foreach$(%obj in SEP_GroundCover.groundCoverList) {
		SEP_GroundCoverMenu.add(%obj.getName(),%obj.getId());

		if (SEP_GroundCover.selectedGroundCover.getId() $= %obj.getId())
			%selected = %obj.getId();
	}

	SEP_GroundCoverMenu.setSelected(%selected,false);
}
//------------------------------------------------------------------------------

//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCoverMenu::onSelect( %this,%id,%text ) {
	logd("SEP_GroundCoverMenu::onSelect( %this,%id,%text )", %this,%id,%text);
	if (isObject(%id))
	SEP_GroundCover.onGroundCoverSelected(%id);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::onGroundCoverSelected( %this,%obj ) {
	logd("SEP_GroundCover know about the selected groundcover:",%obj.getName());
	SceneEditorToolbar-->groundCoverToolbar.visible = 1;
	SEP_GroundCoverDeleteButton.active = true;
	%this.updateGroundCoverLayers(%obj);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::onGroundCoverUnselected( %this,%obj ) {
	logd("SEP_GroundCover know about the unselected groundcover:",%obj.getName());
	SceneEditorToolbar-->groundCoverToolbar.visible = 0;
	hide(SEP_GroundCoverLayerArray);
	hide(SEP_GroundCoverCommonSettings);
}
//------------------------------------------------------------------------------

//==============================================================================
// GroundCover Layers GUI generation
//==============================================================================
$SEP_GroundCoverLayerCount = 8;
%fields = "billboardUVs invertLayer probability shapeFilename shapeFilename_button layer layer_menu windScale";
%fieldsA = "sizeMin sizeMax sizeExponent minSlope maxSlope minElevation maxElevation minClumpCount maxClumpCount clumpExponent clumpRadius";
$SEP_GroundCoverLayerFields = %fields SPC %fieldsA;
%cFields = "Material radius dissolveRadius reflectScale gridSize zOffset seed maxElements maxBillboardTiltAngle shapeCullRadius shapesCastShadows";
%cFieldsA = "windDirection windGustLength windGustFrequency windGustStrength windTurbulenceFrequency windTurbulenceStrength lockFrustum renderCells noBillboards noShapes";
$SEP_GroundCoverCommonFields = %cFields SPC %cFieldsA;
//==============================================================================
// The layers settings Gui is generated from a source Gui structure
function SEP_GroundCover::buildLayerSettingGui( %this ) {
	show(SEP_GroundCoverLayerArray);
	SEP_GroundCoverLayerArray.clear();
	%pillSrc = SEP_GroundCoverLayerPill;

	for(%i=0; %i< $SEP_GroundCoverLayerCount; %i++) {
		%pill = cloneObject(	%pillSrc,"","layer_"@%i,SEP_GroundCoverLayerArray);
		%pill.layerId = %i;

		foreach$(%field in $SEP_GroundCoverLayerFields) {
			eval("%pill-->"@%field@".layerId = %i;");
		}
	}

	hide(SEP_GroundCoverLayerArray);
}
//------------------------------------------------------------------------------
//==============================================================================
// The layers settings Gui is generated from a source Gui structure SEP_GroundCover.updateGroundCoverLayers(newGroundCover)
function SEP_GroundCover::updateGroundCoverLayers( %this,%groundCover ) {
	GroundCoverInspector.inspect(%groundCover);
	SEP_GroundCover.selectedGroundCover = %groundCover.getId();
	SEP_GroundCoverMenu.setText(%groundCover.getName());
	show(SEP_GroundCoverLayerArray);
	show(SEP_GroundCoverCommonSettings);

	foreach$(%field in $SEP_GroundCoverCommonFields) {
		%fieldCtrl = SEP_GroundCoverCommonSettings.findObjectByInternalName(%field,true);

		if (!isObject(%fieldCtrl)) {
			warnLog("Trying to update an invalid control for GroundCover common field:",%field);
			continue;
		}

		%fieldOnly = getWord(strreplace(%field,"_"," "),0);
		eval("%value = %groundCover."@%fieldOnly@";");
		%fieldCtrl.setTypeValue(%value);
	}

	foreach(%obj in SEP_GroundCoverLayerArray) {
		%layerId = %obj.layerId;

		if (%layerId $= "") {
			warnLog("Trying to update an invalid LayerId for GroundCover:",%groundCover.getName());
			continue;
		}

		foreach$(%field in $SEP_GroundCoverLayerFields) {
			%fieldCtrl = %obj.findObjectByInternalName(%field,true);

			if (!isObject(%fieldCtrl)) {
				warnLog("Trying to update an invalid control for GroundCover layer:",%layerId,"Field",%field);
				continue;
			}

			%fieldOnly = getWord(strreplace(%field,"_"," "),0);
			eval("%value = %groundCover."@%fieldOnly@"["@%layerId@"];");
			%fieldCtrl.setTypeValue(%value);
		}
	}
}
//------------------------------------------------------------------------------
//==============================================================================
// GroundCover Settings functions
//==============================================================================
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::selectLayer( %this,%menu ) {
	SEP_GroundCover.currentTerrainLayerId = %menu.layerId;
	materialSelector.showTerrainDialog("SEP_GroundCover.applyLayer", "name");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::applyLayer( %this,%layerName ) {
	%layerId = SEP_GroundCover.currentTerrainLayerId;
	eval("%container = SEP_GroundCoverLayerArray-->layer_"@%layerId@";");
	%fieldCtrl = %container.findObjectByInternalName("layer",true);
	%fieldCtrl.setText(%layerName);
	SEP_GroundCover.updateFieldValue("layer",%layerName,%layerId);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::selectShapeFile( %this,%button ) {
	%current = SEP_GroundCover.selectedGroundCover.getFieldValue("shapeFilename",%button.layerId);
	SEP_GroundCover.currentShapeFileLayerId = %button.layerId;
	getLoadFilename("*.*|*.*", "SEP_GroundCover.applyShapeFile", %current);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::applyShapeFile( %this,%file ) {
	%file = makeRelativePath(%file );
	%layerId = %this.currentShapeFileLayerId;
	eval("%container = SEP_GroundCoverLayerArray-->layer_"@%layerId@";");
	%fieldCtrl = %container.findObjectByInternalName("shapeFilename",true);
	%fieldCtrl.setText(%file);
	SEP_GroundCover.updateFieldValue("shapeFilename",%file,%layerId);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::selectCommonMaterial( %this,%button ) {
	materialSelector.showDialog("SEP_GroundCover.applyCommonMaterial", "name");
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCover::applyCommonMaterial( %this,%materialName ) {
	logd("SEP_GroundCover::applyCommonMaterial( %this,%materialName )",%this,%materialName);
	%fieldCtrl = SEP_GroundCoverCommonSettings.findObjectByInternalName("Material",true);
	%fieldCtrl.setText(%materialName);
	SEP_GroundCover.updateFieldValue("Material",%materialName);
}
//------------------------------------------------------------------------------
//fieldValue = %object.getFieldValue( %fieldName, %arrayIndex );
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCoverEdit::onValidate( %this ) {
	logd("SEP_GroundCoverCheck::onClick( %this )",%this.internalName,"LayerId=",%this.layerId);
	%fieldOnly = getWord(strreplace(%this.internalName,"_"," "),0);
	SEP_GroundCover.updateFieldValue(%fieldOnly,%this.getValue(),%this.layerId);
}
//------------------------------------------------------------------------------
//==============================================================================
// Prepare the default config array for the Scene Editor Plugin
function SEP_GroundCoverCheck::onClick( %this ) {
	logd("SEP_GroundCoverCheck::onClick( %this )",%this.internalName);
}
//------------------------------------------------------------------------------



new GroundCover(envGroundCover)
{
   Material = "fpMeadowA01_baseFarmGrass_MeadowA30";
   radius = "400";
   dissolveRadius = "200";
   maxElements = "90000";
   shapeCullRadius = "400";
   shapeFilename[0] = "art/modelPacks/FarmPack/Nature/Grass/Long/fpGrassLongA_02.DAE";
   shapeFilename[1] = "art/modelPacks/FarmPack/Nature/Grass/Meadow/fpMSparseA_t4_01.DAE";
   shapeFilename[2] = "art/modelPacks/FarmPack/Nature/Trees/MountainAsh/fpMountainAshA_01.DAE";
   shapeFilename[3] = "art/modelPacks/FarmPack/Nature/Flower/Poppy/fpPoppyA_4b_01.dae";
   layer[0] = "gc_GrassSparse_Ground_g2";
   layer[1] = "gc_GrassSparse_Ground_g2";
   layer[2] = "gc_GrassSparse_Ground_g2";
   layer[3] = "gc_GrassSparse_Ground_g2";
   probability[0] = "1";
   probability[1] = "1";
   probability[2] = "1";
   probability[3] = "4";
   sizeMin[3] = "1.4";
   sizeMax[1] = "2";
   sizeMax[3] = "3";
};

new GroundCover(envGroundCover2)
{
   Material = "fpMeadowA01_baseFarmGrass_MeadowA30";
   dissolveRadius = "200";
   maxElements = "30000";
   shapeCullRadius = "400";
   shapeFilename[0] = "art/modelPacks/FarmPack/Nature/Flower/Susan/fpSusanA_4f_01.DAE";
   shapeFilename[1] = "art/modelPacks/FarmPack/Nature/Trees/Alder/fpAlderA_01.DAE";
   shapeFilename[2] = "art/modelPacks/FarmPack/Nature/Rock/MedBoulders/fpMB_BasicA_5.dae";
   layer[0] = "gc_GrassLush_Agrass1";
   layer[1] = "gc_GrassLush_Agrass1";
   layer[2] = "gc_GrassLush_Agrass1";
   probability[0] = "1";
   probability[1] = "0.1";
   probability[2] = "1";
   sizeMax[0] = "2";
};
