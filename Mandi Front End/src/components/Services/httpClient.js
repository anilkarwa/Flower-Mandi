import axios from 'axios'

class Axios {
  constructor () {
    this.axios = axios.create()
    this.axios.defaults.timeout = 2500
  }

  saveItemData (formData) {
    const mainMenuUrl = process.env.BASE_URL + '/api/SaveOrder'
    return this.axios.post(mainMenuUrl, formData, {timeout: 50000})
      .then((Response) => {
        return Response.data
      })
  }
  updateItemData (formData) {
    const mainMenuUrl = process.env.BASE_URL + '/api/SaveOrder'
    return this.axios.put(mainMenuUrl, formData, {timeout: 50000})
      .then((Response) => {
        return Response.data
      })
  }
  updateSingleItem (formData) {
    const mainMenuUrl = process.env.BASE_URL + '/api/updateItem'
    return this.axios.put(mainMenuUrl, formData, {timeout: 50000})
      .then((Response) => {
        return Response.data
      })
  }
  deleteSingleItem (formData) {
    const mainMenuUrl = process.env.BASE_URL + '/api/updateItem'
    return this.axios.delete(mainMenuUrl, formData, {timeout: 50000})
      .then((Response) => {
        return Response.data
      })
  }
  getFarmerList (term) {
    const mainMenuUrl = process.env.BASE_URL + '/api/FarmerList?term=' + term
    return this.axios.get(mainMenuUrl, {timeout: 50000})
      .then((Response) => {
        return Response.data
      })
  }
  getCustomerList (term) {
    const mainMenuUrl = process.env.BASE_URL + '/api/CustomerList?term=' + term
    return this.axios.get(mainMenuUrl, {timeout: 50000})
      .then((Response) => {
        return Response.data
      })
  }
  getItemList (term) {
    const mainMenuUrl = process.env.BASE_URL + '/api/Items?term=' + term
    return this.axios.get(mainMenuUrl, {timeout: 50000})
      .then((Response) => {
        return Response.data
      })
  }
}
export default new Axios()
