<template>

   <v-app >
     <v-toolbar dark color="primary">
      <v-toolbar-title class="white--text">Mandi - {{companyName}}</v-toolbar-title>
      <v-spacer></v-spacer>

      <v-tooltip bottom> <v-btn fab dark small slot="activator" color="pink" @click="userLogout">
        <v-icon dark>power_settings_new</v-icon>
      </v-btn> <span>Logout</span>
      </v-tooltip>
    </v-toolbar>
     <v-container fluid grid-list-xl>
    <v-form ref="form" v-model="valid" lazy-validation>
    <v-layout row >
     <v-flex md3>
        <v-menu
          ref="menu1"
          :close-on-content-click="false"
          v-model="menu1"
          :nudge-right="40"
          lazy
          transition="scale-transition"
          offset-y
          full-width
          max-width="290px"
          min-width="290px"
        >
          <v-text-field
            slot="activator"
            v-model="dateFormatted"
            label="Date"
            hint="DD/MM/YYYY format"
            persistent-hint
            prepend-icon="event"
            @blur="date = parseDate(dateFormatted)"
            :show-current="true"
          ></v-text-field>
          <v-date-picker v-model="date" no-title @input="menu1 = false"></v-date-picker>
        </v-menu>
     </v-flex>
        <v-flex md3>
          <v-select
            :loading="loading"
            :items="farmerList"
            item-text="farmerName"
            item-value="farmerId"
            v-model="selectedFarmer"
            label="Select Farmer"
            autocomplete
            :rules="[v => !!v || 'Select a farmer']"
            :search-input.sync="search"
            cache-items
            required
          ></v-select>
        </v-flex>
    </v-layout>
    <v-layout row>
      <v-flex md3>
        <v-select
          :loading="loading1"
          :items="customerList"
          item-text="customerTabCode"
          item-value="customerId"
          v-model="selectedCustomer"
          label="Select Customer"
          autocomplete
          :hint="customerHintName"
          persistent-hint
          :rules="[v => !!v  || 'Select a customer']"
          :search-input.sync="search1"
          cache-items
          required
          @change="customerHint"
        ></v-select>
      </v-flex>
      <v-flex md3>
        <v-select
          :loading="loading2"
          :items="itemList"
          item-text="itemTabCode"
          item-value="itemId"
          v-model="selectedItem"
          :hint="itemHintName"
          persistent-hint
          label="Select Item"
          autocomplete
          :rules="[v => !!v  || 'Select a item']"
          :search-input.sync="search2"
          cache-items
          required
          @change="itemHint"
        ></v-select>
      </v-flex>
    </v-layout>
    <v-layout row>
      <v-flex md2>
      <v-text-field
        v-model="numberOfBags"
        :rules="[v => !!v  || 'Enter bags quantity',v => /^0*[1-9]\d{0,9}(\.\d{1,2})?%?$/.test(v) || 'Enter number with optional 2 decimals']"
        label="Bags"
        required
      ></v-text-field>
      </v-flex>
      <v-flex md2>
       <v-text-field
        v-model="quantity"
        :rules="[v => !!v || 'Enter quantity',v => /^0*[1-9]\d{0,9}(\.\d{1,2})?%?$/.test(v) || 'Enter number with optional 2 decimals']"
        label="Quantity"
        required
        @change="amount = parseFloat(quantity * purchaseRate).toFixed(2)"
      ></v-text-field>
      </v-flex>
      <v-flex md2>
       <v-text-field
        v-model="purchaseRate"
        :rules="[v => !!v  || 'Enter rate',v => /^0*[1-9]\d{0,9}(\.\d{1,2})?%?$/.test(v) || 'Enter number with optional 2 decimals']"
        label="Purchase Rate"
        required
        @change="amount = parseFloat(quantity * purchaseRate).toFixed(2)"
      ></v-text-field>
      </v-flex>
      <v-flex md2>
       <v-text-field
        v-model="amount"
        :rules="[v => !!v  || 'Enter amount']"
        label="Amount"
        required
        readonly
      ></v-text-field>
      </v-flex>
    </v-layout>
      <v-btn
        :disabled="!valid"
        @click="submitItem"
      >
        Add Item
      </v-btn>
      <v-btn @click="clear">clear</v-btn>
      <v-btn @click="clearLocalStorage">New</v-btn>
    </v-form>

    <v-data-table
      :headers="headers"
      :items="tableItemArrar"
      hide-actions
      class="elevation-1"
    >
      <template slot="items" slot-scope="props">
        <td>{{ props.item.SlNo }}</td>
        <td>{{ props.item.FarmerName }}</td>
        <td>{{ props.item.custName }}</td>
        <td>{{ props.item.itemName }}</td>
        <td>{{ props.item.BQTY }}</td>
        <td>{{ props.item.QTY }}</td>
        <td>{{ props.item.ItemRate }}</td>
        <td>{{ props.item.ItemAmt }}</td>
         <!-- <td class="justify-center layout px-0">
            <v-btn icon class="mx-0" @click="editItem(props.item)">
              <v-icon color="teal">edit</v-icon>
            </v-btn>
            <v-btn icon class="mx-0" @click="deleteItem(props.item)">
              <v-icon color="pink">delete</v-icon>
            </v-btn>
          </td> -->
      </template>
    </v-data-table>
     <v-dialog v-model="dialog" max-width="700px">
        <v-card>
          <v-card-title>
            <span class="headline">Edit Item</span>
          </v-card-title>
          <v-card-text>
            <v-container grid-list-md>
              <v-layout wrap>
                <v-flex md4>
                  <v-select
                    :loading="loading1"
                    :items="customerList"
                    item-text="customerName"
                    item-value="customerId"
                    v-model="editedItem.custId"
                    label="Select Customer"
                    autocomplete
                    :rules="[v => !!v  || 'Select a customer']"
                    :search-input.sync="search1"
                    cache-items
                    required
                  ></v-select>
                </v-flex>
                <v-flex md4>
                  <v-select
                    :loading="loading2"
                    :items="itemList"
                    item-text="itemName"
                    item-value="itemId"
                    v-model="editedItem.itemId"
                    label="Select Item"
                    autocomplete
                    :rules="[v => !!v  || 'Select a item']"
                    :search-input.sync="search2"
                    cache-items
                    required
                  ></v-select>
                </v-flex>
                <v-flex xs12 sm6 md4>
                  <v-text-field v-model="editedItem.BQTY" label="Bags"></v-text-field>
                </v-flex>
                <v-flex xs12 sm6 md4>
                  <v-text-field v-model="editedItem.QTY" label="Quantity"  @change="editedItem.ItemAmt = editedItem.QTY * editedItem.ItemRate"></v-text-field>
                </v-flex>
                <v-flex xs12 sm6 md4>
                  <v-text-field v-model="editedItem.ItemRate" label="Rate"  @change="editedItem.ItemAmt = editedItem.QTY * editedItem.ItemRate"></v-text-field>
                </v-flex>
                <v-flex xs12 sm6 md4>
                  <v-text-field v-model="editedItem.ItemAmt" label="Amount" readonly></v-text-field>
                </v-flex>
              </v-layout>
            </v-container>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue darken-1" flat @click="dialog = false">Cancel</v-btn>
            <v-btn color="blue darken-1" flat @click.native="saveEditedItem">Save</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
      <v-dialog v-model="dialog2" max-width="290">
        <v-card>
          <v-card-title class="headline">Delete Item?</v-card-title>
          <v-card-text>Are you sure, you want to delete this item!</v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="green darken-1" flat="flat" @click.native="dialog2 = false">No,Cancel</v-btn>
            <v-btn color="red darken-1" flat="flat" @click.native="deleteItemAferConfirm">Yes,Delete</v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
     <v-snackbar
        :timeout="timeout"
        :top="y === 'top'"
        :bottom="y === 'bottom'"
        :right="x === 'right'"
        :left="x === 'left'"
        :multi-line="mode === 'multi-line'"
        :vertical="mode === 'vertical'"
        v-model="snackbar"
      >
        {{ text }}
        <v-btn flat color="pink" @click.native="snackbar = false">Close</v-btn>
      </v-snackbar>
     </v-container>
  </v-app>

</template>

<script>
import axios from './Services/httpClient.js'
import router from '../router'
export default {
  data () {
    return {
      date: null,
      dateFormatted: null,
      menu1: false,
      valid: true,
      selectedFarmer: '',
      farmerList: [],
      selectedCustomer: '',
      customerList: [],
      selectedItem: '',
      itemList: [],
      numberOfBags: '1',
      quantity: '',
      purchaseRate: '',
      salesRate: '',
      amount: '',
      otherAmount: '',
      Remarks: '',
      loading: false,
      search: null,
      loading1: false,
      search1: null,
      loading2: false,
      search2: null,
      itemCounter: 0,
      itemDataArray: [],
      headers: [
        { text: 'SlNo', sortable: false, align: 'center' },
        {text: 'Farmer Name', sortable: false, align: 'center'},
        {text: 'Customer Name', sortable: false, align: 'center'},
        {text: 'Item Name', sortable: false, align: 'center'},
        {text: 'BQTY', sortable: false, align: 'center'},
        {text: 'QTY', sortable: false, align: 'center'},
        {text: 'Rate', sortable: false, align: 'center'},
        {text: 'Amount', sortable: false, align: 'center'}
        // { text: 'Actions', value: 'name', sortable: false }
      ],
      tableItemArrar: [],
      editedIndex: -1,
      editedItem: {SlNo: 0, ItemSlNo: 0, QTY: 0, CustID: 0, ItemRate: 0, ItemAmt: 0, BQTY: 0, CustInAmt: 0, ItemID: 0, CustRate: 0},
      snackbar: false,
      y: 'top',
      x: null,
      mode: '',
      timeout: 6000,
      text: '',
      dialog: false,
      dialog2: false,
      deleteItemId: '',
      deleteItemArray: {SlNo: 0, ItemSlNo: 0, QTY: 0, CustID: 0, ItemRate: 0, ItemAmt: 0, BQTY: 0, CustInAmt: 0, ItemID: 0, CustRate: 0},
      itemHintName: 'Select a item',
      customerHintName: 'Select a customer',
      companyName: ''

    }
  },
  beforeMount () {
    this.getCurrentDate()
    this.companyName = localStorage.getItem('companyName')
  },
  computed: {
    computedDateFormatted () {
      return this.formatDate(this.date)
    }
  },
  watch: {
    search (val) {
      val && this.searchFarmer(val)
    },
    search1 (val) {
      val && this.searchCustomer(val)
    },
    search2 (val) {
      val && this.searchItem(val)
    },
    date (val) {
      this.dateFormatted = this.formatDate(this.date)
    }
  },
  methods: {
    formatDate (date) {
      if (!date) return null

      const [year, month, day] = date.split('-')
      return `${day}/${month}/${year}`
    },
    parseDate (date) {
      if (!date) return null

      const [day, month, year] = date.split('/')
      return `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`
    },
    searchFarmer (term) {
      this.farmerList = []
      this.loading = true
      axios.getFarmerList(term).then((data) => {
        data.forEach(element => {
          this.farmerList.push({farmerId: element.FarmerId, farmerName: element.FarmerName, farmerTabCode: element.FarmerTabCode})
        })
        this.loading = false
      })
    },
    searchCustomer (term) {
      this.customerList = []
      this.loading1 = true
      axios.getCustomerList(term).then((data) => {
        data.forEach(element => {
          this.customerList.push({customerId: element.CustId, customerCode: element.CustCode, customerTabCode: element.CustTabCode, customerName: element.CustomerName})
        })
        this.loading1 = false
      })
    },
    searchItem (term) {
      this.itemList = []
      this.loading2 = true
      axios.getItemList(term).then((data) => {
        data.forEach(element => {
          this.itemList.push({itemId: element.ItemId, itemCode: element.ItemCode, itemTabCode: element.ItemTabCode, itemName: element.ItemName})
        })
        this.loading2 = false
      })
    },
    submitItem () {
      this.itemCounter += 1
      let totalQuantity = 0
      let totalAmount = 0
      let grandTotal = 0
      let orderId
      let newItem = {SlNo: this.itemCounter, ItemSlNo: this.itemCounter, QTY: parseFloat(this.quantity).toFixed(2), CustID: this.selectedCustomer, ItemRate: parseFloat(this.purchaseRate).toFixed(2), ItemAmt: parseFloat(this.quantity * this.purchaseRate).toFixed(2), BQTY: parseFloat(this.numberOfBags).toFixed(2), CustInAmt: parseFloat(this.purchaseRate * this.quantity).toFixed(2), ItemID: this.selectedItem, CustRate: parseFloat(this.purchaseRate).toFixed(2), FarmerID: this.selectedFarmer}
      console.log(JSON.stringify(newItem))
      let newTableItemArray = {SlNo: this.itemCounter, itemId: this.selectedItem, itemName: this.itemList.find(e => e.itemId === this.selectedItem).itemName, custId: this.selectedCustomer, custName: this.customerList.find(e => e.customerId === this.selectedCustomer).customerName, QTY: this.quantity, ItemRate: this.purchaseRate, ItemAmt: this.amount, BQTY: this.numberOfBags, FarmerID: this.selectedFarmer, FarmerName: this.farmerList.find(e => e.farmerId).farmerName}
      console.log(newTableItemArray)
      if (localStorage.getItem('itemArray') !== null) {
        this.itemDataArray = JSON.parse(localStorage.getItem('itemArray'))
        this.itemDataArray.push(newItem)
        localStorage.setItem('itemArray', JSON.stringify(this.itemDataArray))
        for (let i = 0; i < this.itemDataArray.length; i++) {
          totalQuantity += parseFloat(this.itemDataArray[i].QTY)
          totalAmount += parseFloat(this.itemDataArray[i].ItemAmt)
          grandTotal += parseFloat(this.itemDataArray[i].ItemAmt)
        }
        orderId = localStorage.getItem('orderId')
        let finalOrder = {SLPID: orderId, SLPDate: this.dateFormatted, TOTQTY: parseFloat(totalQuantity).toFixed(2), TOTAMT: parseFloat(totalAmount).toFixed(2), GRANDTOT: parseFloat(grandTotal).toFixed(2), FarmerId: this.selectedFarmer, RecordAddedBy: this.companyName, Items: this.itemDataArray}
        console.log('Update order format =', finalOrder)
        axios.updateItemData(finalOrder).then((data) => {
          this.snackbar = true
          this.text = data
          this.tableItemArrar.push(newTableItemArray)
        })
        console.log(JSON.stringify(finalOrder))
      } else {
        this.itemDataArray = [newItem]
        localStorage.setItem('itemArray', JSON.stringify(this.itemDataArray))
        let finalOrder = {SLPDate: this.dateFormatted, TOTQTY: parseFloat(this.quantity).toFixed(2), TOTAMT: parseFloat(this.quantity * this.purchaseRate).toFixed(2), GRANDTOT: parseFloat(this.quantity * this.purchaseRate).toFixed(2), FarmerId: this.selectedFarmer, RecordAddedBy: localStorage.getItem('userName'), Items: this.itemDataArray}
        console.log('save new order format', finalOrder)
        axios.saveItemData(finalOrder).then((data) => {
          localStorage.setItem('orderId', data.SLPID)
          this.snackbar = true
          if (data.SLPID !== null) {
            this.text = 'Item Save Sucessfully'
            this.tableItemArrar.push(newTableItemArray)
            console.log('first save=', JSON.stringify(this.tableItemArrar))
          } else {
            this.text = 'Error Saving Item'
          }
        })
        console.log(JSON.stringify(finalOrder))
      }
      this.selectedCustomer = ''
      this.selectedItem = ''
      this.quantity = ''
      this.purchaseRate = ''
      this.amount = ''
      this.numberOfBags = 1
    },
    editItem (item) {
      this.editedIndex = this.tableItemArrar.indexOf(item)
      this.editedItem = Object.assign({}, item)
      this.dialog = true
    },

    deleteItem (item) {
      this.dialog2 = true
      const index = this.tableItemArrar.indexOf(item)
      this.deleteItemArray = Object.assign({}, item)
      this.deleteItemId = index
    },
    deleteItemAferConfirm () {
      axios.deleteSingleItem(this.deleteItemArray).then((data) => {
        this.snackbar = true
        this.text = data
        for (let i = 0; i < this.itemDataArray; i++) {
          if (this.itemDataArray[i].SlNo === this.deleteItemArray.SlNo) {
            this.itemDataArray.splice(i, 1)
          }
          if (this.tableItemArrar[i].SlNo === this.deleteItemArray.SlNo) {
            this.tableItemArrar.splice(i, 1)
          }
        }
        this.dialog2 = false
      })
    },
    clear () {
      this.$refs.form.reset()
    },
    clearLocalStorage () {
      this.tableItemArrar = []
      this.itemCounter = 0
      this.selectedFarmer = ''
      localStorage.removeItem('itemArray')
      localStorage.removeItem('orderId')
    },
    saveEditedItem () {
      this.dialog = false
      console.log(JSON.stringify(this.editedItem))
      let editedItem = {SlNo: this.editedItem.SlNo, itemId: this.editedItem.itemId, itemName: this.itemList.find(e => e.itemId === this.editedItem.itemId).itemName, custId: this.editedItem.custId, custName: this.customerList.find(e => e.customerId === this.editedItem.custId).customerName, QTY: this.editedItem.QTY, ItemRate: this.editedIndex.ItemRate, ItemAmt: this.editedItem.ItemAmt, BQTY: this.editedItem.BQTY}
      for (let i = 0; i < this.tableItemArrar.length; i++) {
        if (this.tableItemArrar[i].SlNo === editedItem.SlNo) {
          this.tableItemArrar[i] = editedItem
        }
      }
      let newItem = {SLPID: localStorage.getItem('orderId'), SlNo: this.editedItem.SlNo, ItemSlNo: this.editedItem.ItemSlNo, QTY: this.editedItem.QTY, CustID: this.editedItem.custId, ItemRate: this.editedItem.ItemRate, ItemAmt: this.editedItem.ItemAmt, BQTY: this.editedItem.BQTY, CustInAmt: 0, ItemID: this.editedItem.itemId, CustRate: this.editedItem.ItemRate}
      this.itemDataArray = JSON.parse(localStorage.getItem('itemArray'))
      for (let i = 0; i < this.itemDataArray.length; i++) {
        if (this.itemDataArray[i].SlNo === newItem.SlNo) {
          this.itemDataArray[i] = newItem
        }
      }
      axios.updateSingleItem(newItem).then((data) => {
        this.snackbar = true
        this.text = data
      })
      localStorage.setItem('itemArray', JSON.stringify(this.itemDataArray))
    },
    getCurrentDate () {
      let today = new Date()
      let dd = today.getDate()
      let mm = today.getMonth() + 1
      let yyyy = today.getFullYear()
      if (dd < 10) {
        dd = '0' + dd
      }
      if (mm < 10) {
        mm = '0' + mm
      }
      const todayDate = dd + '/' + mm + '/' + yyyy
      this.dateFormatted = todayDate
    },
    customerHint () {
      setTimeout(() => {
        this.customerHintName = this.customerList.find(e => e.customerId === this.selectedCustomer).customerName
      }, 500)
    },
    itemHint () {
      setTimeout(() => {
        this.itemHintName = this.itemList.find(e => e.itemId === this.selectedItem).itemName
      }, 500)
    },
    userLogout () {
      localStorage.removeItem('userLoggedIn')
      this.categoryName = 'Login'
      router.push({name: 'Login', params: {categoryName: this.categoryName}})
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>

</style>
