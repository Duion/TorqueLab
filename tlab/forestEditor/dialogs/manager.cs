//==============================================================================
// TorqueLab ->
// Copyright (c) 2015 All Right Reserved, http://nordiklab.com/
//------------------------------------------------------------------------------
//==============================================================================

//==============================================================================
function FEP_Manager::onWake( %this ) {
	devLog("FEP_Manager::onWake");
	%this-->TabBook.selectPage(0);
	%this.loadData();
	%this-->saveChangesButton.active = ForestDataManager.dirty;
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::init( %this ) {
	FEP_Manager.initDataGenerator();
	%this-->brush_filters.setText("Filters...");
	%this.filters["brush"] = "";
}
//------------------------------------------------------------------------------
//==============================================================================
function FEP_Manager::loadData( %this ) {
	ForestManagerBrushTree.open( ForestBrushGroup );
	ForestManagerItemTree.open( ForestItemDataSet );
	%this.dataTree["brush"] = ForestManagerBrushTree;
	%this.dataTree["item"] = ForestManagerItemTree;
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_Manager::saveData( %this ) {
	ForestDataManager.saveDirty();
	ForestDataManager.dirty = false;
	%this-->saveChangesButton.active = false;
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_Manager::setDirty( %this ) {
	ForestDataManager.dirty = true;
	%this-->saveChangesButton.active = true;
}
//------------------------------------------------------------------------------

//==============================================================================
function FEP_TreeFilters::onValidate( %this ) {
	%typeData = strreplace(%this.internalName,"_"," ");
	%type = getWord(%typeData,0);
	FEP_Manager.filters[%type] = %this.getText();

	if (strFind(%this.filters[%type],"Filters..."))
		FEP_Manager.filters[%type] = "";

	FEP_Manager.dataTree[%type].setFilterText(FEP_Manager.filters[%type]);
}
//------------------------------------------------------------------------------

