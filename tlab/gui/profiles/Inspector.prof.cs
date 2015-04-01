//==============================================================================
// Castle Blasters ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
singleton GuiControlProfile( GuiInspectorProfile  : ToolsDefaultProfile ) {
    opaque = true;
    fillColor = "255 255 255 255";
    border = 0;
    cankeyfocus = true;
    tab = true;
    category = "Editor";
};
//------------------------------------------------------------------------------



//==============================================================================
//Used in SourceCode
singleton GuiControlProfile( GuiInspectorButtonProfile : ToolsGuiButtonProfile ) {
    //border = 1;
     fontSize = "16";
    fontType = "Gafata Std Bold";
    fontColor = "254 254 254 255";
    justify = "center";
    opaque = "1";
    border = "1";
    fontColors[0] = "254 254 254 255";
    fontColors[2] = "200 200 200 255";
    fontColorNA = "200 200 200 255";
    bitmap = "tlab/gui/images/button_assets/GuiButtonMain.png";
    fixedExtent = "0";
   bevelColorLL = "Magenta";
   textOffset = "0 2";
   autoSizeWidth = "1";
   autoSizeHeight = "1";
    category = "Editor";
};
//------------------------------------------------------------------------------
//==============================================================================
//Used in SourceCode
singleton GuiControlProfile(GuiInspectorSwatchButtonProfile : GuiInspectorButtonProfile)
{
	fillColor = "254 253 253 255";
	fillColorHL = "221 221 221 255";
	fillColorNA = "200 200 200 255";
	fontSize = "24";
	textOffset = "16 10";
	bitmap = "tlab/gui/images/button_assets/GuiButtonMain.png";
	hasBitmapArray = "1";
	fontColors[0] = "253 253 253 255";
	fontColor = "253 253 253 255";
   border = "-1";
   fontColors[2] = "Black";
   fontColorNA = "Black";
};
//------------------------------------------------------------------------------

//==============================================================================
// Inspector Text Profiles
//==============================================================================

//==============================================================================
// SourceCode TextEdit Profile
singleton GuiControlProfile( GuiInspectorTextEditProfile ) {
    borderColor = "100 100 100 255";
    borderColorNA = "75 75 75 255";
    fillColorNA = "White";
    borderColorHL = "50 50 50 50";
    category = "Editor";
    tab = "1";
    canKeyFocus = "1";
    opaque = "1";
    fillColor = "0 0 0 0";
    fillColorHL = "2 2 2 49";
    fontType = "Arial";
    fontColors[1] = "254 254 254 255";
    fontColors[2] = "100 100 100 255";
    fontColors[3] = "16 108 87 255";
    fontColors[9] = "255 0 255 255";
    fontColorHL = "254 254 254 255";
    fontColorNA = "100 100 100 255";
    fontColorSEL = "16 108 87 255";
    fontColors[0] = "231 224 178 255";
    fontColor = "231 224 178 255";
};
//------------------------------------------------------------------------------
//==============================================================================
singleton GuiControlProfile( GuiDropdownTextEditProfile :  GuiInspectorTextEditProfile ) {
    bitmap = "tlab/gui/oldImages/dropdown-textEdit";
    category = "Editor";
};
//------------------------------------------------------------------------------

singleton GuiControlProfile( GuiInspectorFieldInfoMLTextProfile : ToolsDefaultProfile ) {
    opaque = false;
    border = 0;
    textOffset = "5 0";
    category = "Editor";
};


//==============================================================================
// Inspector Group Profiles
//==============================================================================
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorGroupProfile : ToolsDefaultProfile ) {
    justify = "Left";
    category = "Editor";
    fontType = "Gotham Bold";
    fontColors[0] = "238 238 238 255";
    fontColors[1] = "25 25 25 220";
    fontColors[2] = "128 128 128 255";
    fontColors[9] = "255 0 255 255";
    fontColor = "238 238 238 255";
    fontColorHL = "25 25 25 220";
    fontColorNA = "128 128 128 255";
    textOffset = "20 0";
    bitmap = "tlab/gui/images/container_assets/GuiRolloutMain.png";
    opaque = "0";
    fillColor = "0 0 0 237";
    fillColorNA = "255 255 255 255";
    fontSize = "15";
   fillColorHL = "255 255 255 255";
   fontColors[3] = "43 107 206 255";
   fontColorSEL = "43 107 206 255";
};
//------------------------------------------------------------------------------

//==============================================================================
// Inspector Fields Profiles
//==============================================================================
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorFieldProfile ) {
    fontType    = "Arial";
    fontSize    = "14";

    fontColor = "238 238 238 255";
    fontColorHL = "231 224 178 255";
    fontColorNA = "190 190 190 255";

    justify = "left";
    opaque = false;
    border = false;

    bitmap = "tlab/editorclasses/gui/images/rollout";

    textOffset = "10 0";

    category = "Editor";
    tab = "1";
    canKeyFocus = "1";
    fillColor = "51 51 51 255";
    fillColorHL = "32 32 34 255";
    fillColorNA = "244 244 244 255";
    borderColor = "190 190 190 255";
    borderColorHL = "156 156 156 255";
    borderColorNA = "200 200 200 255";
    fontColors[0] = "238 238 238 255";
    fontColors[1] = "231 224 178 255";
    fontColors[2] = "190 190 190 255";
    fontColors[9] = "255 0 255 255";
};
//------------------------------------------------------------------------------
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorMultiFieldProfile : GuiInspectorFieldProfile )
{
   opaque = true;
   fillColor = "50 50 230 30";
};
//------------------------------------------------------------------------------
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorDynamicFieldProfile : GuiInspectorFieldProfile ) {
    border = true;
    borderColor = "190 190 190 255";
    opaque = "1";
    fillColor = "51 51 51 255";
    fillColorHL = "32 32 34 255";
    fontColors[0] = "238 238 238 255";
    fontColors[1] = "231 224 178 255";
    fontColors[2] = "100 100 100 255";
    fontColors[3] = "43 107 206 255";
    fontColor = "238 238 238 255";
    fontColorHL = "231 224 178 255";
    fontColorNA = "100 100 100 255";
    fontColorSEL = "43 107 206 255";
};
//------------------------------------------------------------------------------

//==============================================================================
// Used in SourceCode -> Rollout for Array settings (Ex: GroundCover Layers)
singleton GuiControlProfile( GuiInspectorRolloutProfile0 ) {
    // font
    fontType = "Gotham Book";
    fontSize = 14;

    fontColor = "32 32 32";
    fontColorHL = "32 100 100";
    fontColorNA = "0 0 0";

    justify = "left";
    opaque = false;

    border = 0;
    borderColor   = "190 190 190";
    borderColorHL = "156 156 156";
    borderColorNA = "64 64 64";

    bitmap = "tlab/editorclasses/gui/images/rollout_plusminus_header";

    textOffset = "20 0";
    category = "Editor";
    fontColors[0] = "32 32 32 255";
    fontColors[1] = "32 100 100 255";
    fontColors[9] = "255 0 255 255";
};
//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( GuiInspectorStackProfile ) {
    opaque = "1";
    border = false;
    category = "Editor";
    fillColor = "51 51 51 255";
    fontColors[6] = "255 0 255 255";
    fillColorHL = "32 32 34 255";
    fontColors[0] = "238 238 238 255";
    fontColor = "238 238 238 255";
    fontColors[1] = "231 224 178 255";
    fontColorHL = "231 224 178 255";
};
//------------------------------------------------------------------------------


//==============================================================================
// Used in SourceCode
singleton GuiControlProfile( InspectorTypeCheckboxProfile : GuiInspectorFieldProfile ) {
    bitmap = "tlab/gui/oldImages/checkBox";
    hasBitmapArray = true;
    opaque=false;
    border=false;
    textOffset = "4 0";
    category = "Editor";
};
//------------------------------------------------------------------------------






//==============================================================================
// Gui Inspector Profiles
//==============================================================================






//==============================================================================
// Push, Pop and Toggle Dialogs
//==============================================================================



