//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
//=======================================
// Material Update Functionality

function MaterialEditorGui::changeLayer( %this, %layer ) {
    if( MaterialEditorGui.currentLayer == getWord(%layer, 1) )
        return;

    MaterialEditorGui.currentLayer = getWord(%layer, 1);
    MaterialEditorGui.guiSync( materialEd_previewMaterial );
}


function MaterialEditorGui::updateMaterialReferences( %this, %obj, %oldName, %newName ) {
    if ( %obj.isMemberOfClass( "SimSet" ) ) {
        // invoke on children
        %count = %obj.getCount();
        for ( %i = 0; %i < %count; %i++ )
            %this.updateMaterialReferences( %obj.getObject( %i ), %oldName, %newName );
    } else {
        %objChanged = false;

        // Change all material fields that use the old material name
        %count = %obj.getFieldCount();
        for( %i = 0; %i < %count; %i++ ) {
            %fieldName = %obj.getField( %i );
            if ( ( %obj.getFieldType( %fieldName ) $= "TypeMaterialName" ) && ( %obj.getFieldValue( %fieldName ) $= %oldName ) ) {
                eval( %obj @ "." @ %fieldName @ " = " @ %newName @ ";" );
                %objChanged = true;
            }
        }

        EWorldEditor.isDirty |= %objChanged;
        if ( %objChanged && %obj.isMethod( "postApply" ) )
            %obj.postApply();
    }
}

// Global Material Options

function MaterialEditorGui::updateReflectionType( %this, %type ) {
    if( %type $= "None" ) {
        MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(0);
        //Reset material reflection settings on the preview materials
        MaterialEditorGui.updateActiveMaterial( "cubeMap", "" );
        MaterialEditorGui.updateActiveMaterial( "dynamicCubemap" , false );
        MaterialEditorGui.updateActiveMaterial( "planarReflection", false );
    } else {
        if(%type $= "cubeMap") {
            MaterialEditorPropertiesWindow-->matEd_cubemapEditBtn.setVisible(1);
            MaterialEditorGui.updateActiveMaterial( %type, materialEd_previewMaterial.cubemap );
        } else {
            MaterialEditorGui.updateActiveMaterial( %type, true );
        }
    }
}

// Per-Layer Material Options

// For update maps
// %action : 1 = change map
// %action : 0 = remove map

function MaterialEditorGui::updateTextureMap( %this, %type, %action ) {
    %layer = MaterialEditorGui.currentLayer;

    %bitmapCtrl = MaterialEditorPropertiesWindow.findObjectByInternalName( %type @ "MapDisplayBitmap", true );
    %textCtrl = MaterialEditorPropertiesWindow.findObjectByInternalName( %type @ "MapNameText", true );

    if( %action ) {
        %texture = MaterialEditorGui.openFile("texture");
        if( %texture !$= "" ) {
            %bitmapCtrl.setBitmap(%texture);

            %bitmap = %bitmapCtrl.bitmap;
            %bitmap = strreplace(%bitmap,"tlab/materialEditor/scripts/","");
            %bitmapCtrl.setBitmap(%bitmap);
            %textCtrl.setText(%bitmap);
            MaterialEditorGui.updateActiveMaterial(%type @ "Map[" @ %layer @ "]","\"" @ %bitmap @ "\"");
        }
    } else {
        %textCtrl.setText("None");
        %bitmapCtrl.setBitmap("tlab/materialEditor/assets/unknownImage");
        MaterialEditorGui.updateActiveMaterial(%type @ "Map[" @ %layer @ "]","");
    }
}

function MaterialEditorGui::updateDetailScale(%this,%newScale) {
    %layer = MaterialEditorGui.currentLayer;

    %detailScale = "\"" @ %newScale SPC %newScale @ "\"";
    MaterialEditorGui.updateActiveMaterial("detailScale[" @ %layer @ "]", %detailScale);
}

function MaterialEditorGui::updateDetailNormalStrength(%this,%newStrength) {
    %layer = MaterialEditorGui.currentLayer;

    %detailStrength = "\"" @ %newStrength @ "\"";
    MaterialEditorGui.updateActiveMaterial("detailNormalMapStrength[" @ %layer @ "]", %detailStrength);
}

function MaterialEditorGui::updateSpecMap(%this,%action) {
    %layer = MaterialEditorGui.currentLayer;

    if( %action ) {
        %texture = MaterialEditorGui.openFile("texture");
        if( %texture !$= "" ) {
            MaterialEditorGui.updateActiveMaterial("pixelSpecular[" @ MaterialEditorGui.currentLayer @ "]", 0);

            MaterialEditorPropertiesWindow-->specMapDisplayBitmap.setBitmap(%texture);

            %bitmap = MaterialEditorPropertiesWindow-->specMapDisplayBitmap.bitmap;
            %bitmap = strreplace(%bitmap,"tlab/materialEditor/scripts/","");
            MaterialEditorPropertiesWindow-->specMapDisplayBitmap.setBitmap(%bitmap);
            MaterialEditorPropertiesWindow-->specMapNameText.setText(%bitmap);
            MaterialEditorGui.updateActiveMaterial("specularMap[" @ %layer @ "]","\"" @ %bitmap @ "\"");
        }
    } else {
        MaterialEditorPropertiesWindow-->specMapNameText.setText("None");
        MaterialEditorPropertiesWindow-->specMapDisplayBitmap.setBitmap("tlab/materialEditor/assets/unknownImage");
        MaterialEditorGui.updateActiveMaterial("specularMap[" @ %layer @ "]","");
    }

    MaterialEditorGui.guiSync( materialEd_previewMaterial );
}

function MaterialEditorGui::updateRotationOffset(%this, %isSlider, %onMouseUp) {
    %layer = MaterialEditorGui.currentLayer;
    %X = MaterialEditorPropertiesWindow-->RotationTextEditU.getText();
    %Y = MaterialEditorPropertiesWindow-->RotationTextEditV.getText();
    MaterialEditorPropertiesWindow-->RotationCrosshair.setPosition(45*mAbs(%X)-2, 45*mAbs(%Y)-2);

    MaterialEditorGui.updateActiveMaterial("rotPivotOffset[" @ %layer @ "]","\"" @ %X SPC %Y @ "\"",%isSlider,%onMouseUp);
}

function MaterialEditorGui::updateRotationSpeed(%this, %isSlider, %onMouseUp) {
    %layer = MaterialEditorGui.currentLayer;
    %speed = MaterialEditorPropertiesWindow-->RotationSpeedTextEdit.getText();
    MaterialEditorGui.updateActiveMaterial("rotSpeed[" @ %layer @ "]",%speed,%isSlider,%onMouseUp);
}

function MaterialEditorGui::updateScrollOffset(%this, %isSlider, %onMouseUp) {
    %layer = MaterialEditorGui.currentLayer;
    %X = MaterialEditorPropertiesWindow-->ScrollTextEditU.getText();
    %Y = MaterialEditorPropertiesWindow-->ScrollTextEditV.getText();
    MaterialEditorPropertiesWindow-->ScrollCrosshair.setPosition( -(23 * %X)+20, -(23 * %Y)+20);

    MaterialEditorGui.updateActiveMaterial("scrollDir[" @ %layer @ "]","\"" @ %X SPC %Y @ "\"",%isSlider,%onMouseUp);
}

function MaterialEditorGui::updateScrollSpeed(%this, %isSlider, %onMouseUp) {
    %layer = MaterialEditorGui.currentLayer;
    %speed = MaterialEditorPropertiesWindow-->ScrollSpeedTextEdit.getText();
    MaterialEditorGui.updateActiveMaterial("scrollSpeed[" @ %layer @ "]",%speed,%isSlider,%onMouseUp);
}

function MaterialEditorGui::updateWaveType(%this) {
    for( %radioButton = 0; %radioButton < MaterialEditorPropertiesWindow-->WaveButtonContainer.getCount(); %radioButton++ ) {
        if( MaterialEditorPropertiesWindow-->WaveButtonContainer.getObject(%radioButton).getValue() == 1 )
            %type = MaterialEditorPropertiesWindow-->WaveButtonContainer.getObject(%radioButton).waveType;
    }

    %layer = MaterialEditorGui.currentLayer;
    MaterialEditorGui.updateActiveMaterial("waveType[" @ %layer @ "]", %type);
}

function MaterialEditorGui::updateWaveAmp(%this, %isSlider, %onMouseUp) {
    %layer = MaterialEditorGui.currentLayer;
    %amp = MaterialEditorPropertiesWindow-->WaveTextEditAmp.getText();
    MaterialEditorGui.updateActiveMaterial("waveAmp[" @ %layer @ "]", %amp, %isSlider, %onMouseUp);
}

function MaterialEditorGui::updateWaveFreq(%this, %isSlider, %onMouseUp) {
    %layer = MaterialEditorGui.currentLayer;
    %freq = MaterialEditorPropertiesWindow-->WaveTextEditFreq.getText();
    MaterialEditorGui.updateActiveMaterial("waveFreq[" @ %layer @ "]", %freq, %isSlider, %onMouseUp);
}

function MaterialEditorGui::updateSequenceFPS(%this, %isSlider, %onMouseUp) {
    %layer = MaterialEditorGui.currentLayer;
    %fps = MaterialEditorPropertiesWindow-->SequenceTextEditFPS.getText();
    MaterialEditorGui.updateActiveMaterial("sequenceFramePerSec[" @ %layer @ "]", %fps, %isSlider, %onMouseUp);
}

function MaterialEditorGui::updateSequenceSSS(%this, %isSlider, %onMouseUp) {
    %layer = MaterialEditorGui.currentLayer;
    %sss = 1 / MaterialEditorPropertiesWindow-->SequenceTextEditSSS.getText();
    MaterialEditorGui.updateActiveMaterial("sequenceSegmentSize[" @ %layer @ "]", %sss, %isSlider, %onMouseUp);
}

function MaterialEditorGui::updateAnimationFlags(%this) {
    MaterialEditorGui.setMaterialDirty();
    %single = true;

    if(MaterialEditorPropertiesWindow-->RotationAnimation.getValue() == true) {
        if(%single == true)
            %flags = %flags @ "$Rotate";
        else
            %flags = %flags @ " | $Rotate";

        %single = false;
    }
    if(MaterialEditorPropertiesWindow-->ScrollAnimation.getValue() == true) {
        if(%single == true)
            %flags = %flags @ "$Scroll";
        else
            %flags = %flags @ " | $Scroll";

        %single = false;
    }
    if(MaterialEditorPropertiesWindow-->WaveAnimation.getValue() == true) {
        if(%single == true)
            %flags = %flags @ "$Wave";
        else
            %flags = %flags @ " | $Wave";

        %single = false;
    }
    if(MaterialEditorPropertiesWindow-->ScaleAnimation.getValue() == true) {
        if(%single == true)
            %flags = %flags @ "$Scale";
        else
            %flags = %flags @ " | $Scale";

        %single = false;
    }
    if(MaterialEditorPropertiesWindow-->SequenceAnimation.getValue() == true) {
        if(%single == true)
            %flags = %flags @ "$Sequence";
        else
            %flags = %flags @ " | $Sequence";

        %single = false;
    }

    if(%flags $= "")
        %flags = "\"\"";

    %action = %this.createUndo(ActionUpdateActiveMaterialAnimationFlags, "Update Active Material");
    %action.material = MaterialEditorGui.currentMaterial;
    %action.object = MaterialEditorGui.currentObject;
    %action.layer = MaterialEditorGui.currentLayer;

    %action.newValue = %flags;

    %oldFlags = MaterialEditorGui.currentMaterial.getAnimFlags(MaterialEditorGui.currentLayer);
    if(%oldFlags $= "")
        %oldFlags = "\"\"";

    %action.oldValue = %oldFlags;
    MaterialEditorGui.submitUndo( %action );

    eval("materialEd_previewMaterial.animFlags[" @ MaterialEditorGui.currentLayer @ "] = " @ %flags @ ";");
    materialEd_previewMaterial.flush();
    materialEd_previewMaterial.reload();

    if (MaterialEditorGui.livePreview == true) {
        eval("MaterialEditorGui.currentMaterial.animFlags[" @ MaterialEditorGui.currentLayer @ "] = " @ %flags @ ";");
        MaterialEditorGui.currentMaterial.flush();
        MaterialEditorGui.currentMaterial.reload();
    }
}


//These two functions are focused on object/layer specific functionality
function MaterialEditorGui::updateColorMultiply(%this,%color) {
    %propName = "diffuseColor[" @ MaterialEditorGui.currentLayer @ "]";
    %this.syncGuiColor(MaterialEditorPropertiesWindow-->colorTintSwatch, %propName, %color);
}

function MaterialEditorGui::updateSpecularCheckbox(%this,%value) {
    MaterialEditorGui.updateActiveMaterial("pixelSpecular[" @ MaterialEditorGui.currentLayer @ "]", %value);
    MaterialEditorGui.guiSync( materialEd_previewMaterial );
}

function MaterialEditorGui::updateSpecular(%this, %color) {
    %propName = "specular[" @ MaterialEditorGui.currentLayer @ "]";
    %this.syncGuiColor(MaterialEditorPropertiesWindow-->specularColorSwatch, %propName, %color);
}

function MaterialEditorGui::updateSubSurfaceColor(%this, %color) {
    %propName = "subSurfaceColor[" @ MaterialEditorGui.currentLayer @ "]";
    %this.syncGuiColor(MaterialEditorPropertiesWindow-->subSurfaceColorSwatch, %propName, %color);
}

function MaterialEditorGui::updateEffectColor0(%this, %color) {
    %this.syncGuiColor(MaterialEditorPropertiesWindow-->effectColor0Swatch, "effectColor[0]", %color);
}

function MaterialEditorGui::updateEffectColor1(%this, %color) {
    %this.syncGuiColor(MaterialEditorPropertiesWindow-->effectColor1Swatch, "effectColor[1]", %color);
}

function MaterialEditorGui::updateBehaviorSound(%this, %type, %sound) {
    %defaultId = -1;
    %customName = "";

    switch$ (%sound) {
    case "<Soft>":
        %defaultId = 0;
    case "<Hard>":
        %defaultId = 1;
    case "<Metal>":
        %defaultId = 2;
    case "<Snow>":
        %defaultId = 3;
    default:
        %customName = %sound;
    }

    %this.updateActiveMaterial(%type @ "SoundId", %defaultId);
    %this.updateActiveMaterial("custom" @ %type @ "Sound", %customName);
}

function MaterialEditorGui::updateSoundPopup(%this, %type, %defaultId, %customName) {
    %ctrl = MaterialEditorPropertiesWindow.findObjectByInternalName( %type @ "SoundPopup", true );

    switch (%defaultId) {
    case 0:
        %name = "<Soft>";
    case 1:
        %name = "<Hard>";
    case 2:
        %name = "<Metal>";
    case 3:
        %name = "<Snow>";
    default:
        if (%customName $= "")
            %name = "<None>";
        else
            %name = %customName;
    }

    %r = %ctrl.findText(%name);
    if (%r != -1)
        %ctrl.setSelected(%r, false);
    else
        %ctrl.setText(%name);
}

//These two functions are focused on environment specific functionality
function MaterialEditorGui::updateLightColor(%this, %color) {
    matEd_previewObjectView.setLightColor(%color);
    matEd_lightColorPicker.color = %color;
}

function MaterialEditorGui::updatePreviewBackground(%this,%color) {
    matEd_previewBackground.color = %color;
    MaterialPreviewBackgroundPicker.color = %color;
}

function MaterialEditorGui::updateAmbientColor(%this,%color) {
    matEd_previewObjectView.setAmbientLightColor(%color);
    matEd_ambientLightColorPicker.color = %color;
}