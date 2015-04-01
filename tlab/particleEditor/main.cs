//==============================================================================
// Lab Editor ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================
// Initialization and shutdown code for particle editor plugin.


//---------------------------------------------------------------------------------------------

function initializeParticleEditor() {
    echo( " % - Initializing Particle Editor" );

    exec( "./gui/ParticleEditor.gui" );
    exec( "./scripts/particleEditor.cs" );
    exec( "./scripts/particleEditorUndo.cs" );
    exec( "./scripts/particleEmitterEditor.cs" );
    exec( "./scripts/particleParticleEditor.cs" );

    exec( "tlab/particleEditor/ParticleEditorPlugin.cs" );
    exec( "tlab/particleEditor/ParticleEditorParams.cs" );



    Lab.addPluginGui("ParticleEditor",PE_Window);

    Lab.createPlugin("ParticleEditor");
    ParticleEditorPlugin.superClass = "WEditorPlugin";
    ParticleEditorPlugin.customPalette = "SceneEditorPalette";


    %map = new ActionMap();
    %map.bindCmd( keyboard, "1", "EWorldEditorNoneModeBtn.performClick();", "" );  // Select
    %map.bindCmd( keyboard, "2", "EWorldEditorMoveModeBtn.performClick();", "" );  // Move
    %map.bindCmd( keyboard, "3", "EWorldEditorRotateModeBtn.performClick();", "" );  // Rotate
    %map.bindCmd( keyboard, "4", "EWorldEditorScaleModeBtn.performClick();", "" );  // Scale

    ParticleEditorPlugin.map = %map;

    new ScriptObject( ParticleEditor );

    new PersistenceManager( PE_EmitterSaver );
    new PersistenceManager( PE_ParticleSaver );

    new SimSet( PE_UnlistedParticles );
    new SimSet( PE_UnlistedEmitters );
}

//---------------------------------------------------------------------------------------------

function destroyParticleEditor() {
}
