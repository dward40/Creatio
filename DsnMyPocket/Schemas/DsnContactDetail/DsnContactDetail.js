define("DsnContactDetail", ["LookupMultiAddMixin"], function() {
	return {
		entitySchemaName: "DsnFavoritePokemonDatail",
        mixins: {
            LookupMultiAddMixin: "Terrasoft.LookupMultiAddMixin"
        },
		messages: {
			"GetContactSex": {
				mode: Terrasoft.MessageMode.PTP,
				direction: Terrasoft.MessageDirectionType.PUBLISH
			},
			
   			"CountDetail": {
       			mode: Terrasoft.MessageMode.PTP,
        		direction: Terrasoft.MessageDirectionType.SUBSCRIBE
    },
			
		},
        methods: {

            init: function() {
                this.callParent(arguments);
                this.mixins.LookupMultiAddMixin.init.call(this);
				this.sandbox.subscribe("CountDetail", this.onMessageCountSubscribe, this, ["CountDetail"]);
				//debugger;
				var sex = this.sandbox.publish("GetContactSex", {}, ["GetContactSex"]);
				//debugger;
            },
			onMessageCountSubscribe: function(args){
				
			return this.$Collection.collection.items.length;
			},
            getAddRecordButtonVisible: function() {
				var bool = (this.get("CardPageName") == "ContactPageV2") ? true:false;
                return bool;
            },
            onCardSaved: function() {
                this.openLookupWithMultiSelect();
            },
            addRecord: function() {
                this.openLookupWithMultiSelect(true);
            },
            getMultiSelectLookupConfig: function() {
                return {
                    rootEntitySchemaName: "Contact",
                    rootColumnName: "DsnLookupContact",
                    relatedEntitySchemaName: "DsnPokemons",
                    relatedColumnName: "DsnLookupPokemon"
                };
            }
        }
	};
});
