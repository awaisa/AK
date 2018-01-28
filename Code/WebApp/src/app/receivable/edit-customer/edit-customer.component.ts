import { Component, OnInit, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { InventoryService } from '../../inventory/inventory.service';
import { vendor, Party, Contacts} from '../../entities';
import { ReceivableService } from '../receivable.service';
import { ErrorInfo } from "../../shared/ErrorInfo";
//declare var $:any ;
declare var $: any;
declare var toastr: any;
declare var window: any;

@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.css']
})
export class EditCustomerComponent implements OnInit {

  constructor(private route: ActivatedRoute,
    private router: Router,
    private inventoryService: InventoryService,
    private receivableService: ReceivableService
  ) {
  }

  loaded = false;
  aniFrame = 'in';
  error;
  _error: ErrorInfo = new ErrorInfo();
  vendorPartyForm: FormGroup;
  contactVendorPartyForm: FormGroup;
  vendorForm: FormGroup;

  accounts: any = [];
  vendorParty: Party;
  vendor: vendor;
  contactVendorParty: Contacts;
  partyContacts: Array<Contacts> = [];

  addContactFlag = false;

  ngOnInit() {
    this.initForm();
    this.initObject();
    this.fetchAccounts();
    this.initContactForm()
  }
  // fetch Object starts
  fetchAccounts() {
    this.inventoryService.getAccounts()
      .subscribe(result => {
        this.accounts = result;
        this.loaded = true;
      },
      err => {
        this.error.error(err);
      });
  }
// fetch object ends

  addContact() {
    this.addContactFlag = true;
    this.initContactForm();
  }
  cancle() {
    this.addContactFlag = false;
  }
  // submit starts
  addVendor() {
    if (!this.addContactFlag) {
      if(this.validatePartyForm()) {
        this.receivableService.saveVendor(this.vendor)
        .subscribe(data => {
          const _vendor = data;
          debugger
        }, error => {
          let _error = error
          _error = _error.response.json();
          this._error = error;
          this.checkErrors(_error);
          console.log(this.vendor);
          debugger
        })
      }
    } else {
      toastr.warning('Form is invalid, Save all changes!', 'Oops');
    }
  }
  checkErrors(_err) {
    let keys = Object.keys(_err);
    keys.forEach((item, index) => {
      debugger
      let s = item.split('.');
      if(s.length > 2 && s[1].indexOf('contacts') !== -1) {
        let _index = Number(s[1].charAt(9));
        this.vendor.party.contacts[_index][s[2]] = '';
        this.contactVendorPartyForm.get(s[2]).markAsDirty();
        this.contactVendorPartyForm.get(s[2]).markAsTouched();
      } else if (s.length > 1 && s[1].indexOf('contacts') === -1) {
        this.vendor.party[s[1]] = '';
        this.vendorPartyForm.get(s[1]).markAsDirty();
        this.vendorPartyForm.get(s[1]).markAsTouched();
      } else {
        this.vendor[s[0]] = '';
        this.vendorPartyForm.get(s[0]).markAsDirty();
        this.vendorPartyForm.get(s[0]).markAsTouched();
      }
      })
  }
  submitContact() {
    if(this.validateContactForm()) {
      this.partyContacts.push(Object.assign({}, this.contactVendorParty));
      this.cancle();
    }
  }
  // submit ends
  // validation starts
  validateContactForm() {
    if(this.contactVendorPartyForm.get('firstName').status === 'VALID') {
      this.contactVendorParty.firstName = this.contactVendorPartyForm.get('firstName').value;
    } else {
      this.contactVendorPartyForm.get('firstName').markAsTouched();
      this.contactVendorPartyForm.get('firstName').markAsDirty();
      toastr.warning('First name is required');
      return;
    }
    if(this.contactVendorPartyForm.get('middleName').status === 'VALID') {
      this.contactVendorParty.middleName = this.contactVendorPartyForm.get('middleName').value;
    } else {
      this.contactVendorPartyForm.get('middleName').markAsTouched();
      this.contactVendorPartyForm.get('middleName').markAsDirty();
      toastr.warning('Middle name is required');
      return;
    }
    if(this.contactVendorPartyForm.get('lastName').status === 'VALID') {
      this.contactVendorParty.lastName = this.contactVendorPartyForm.get('lastName').value;
    } else {
      this.contactVendorPartyForm.get('lastName').markAsTouched();
      this.contactVendorPartyForm.get('lastName').markAsDirty();
      toastr.warning('Surname is required');
      return;
    }
    return true;
  }
  validatePartyForm() {
    if(this.vendorPartyForm.get('name').status === 'VALID') {
      this.vendorParty.name = this.vendorPartyForm.get('name').value;
    } else {
      this.vendorPartyForm.get('name').markAsTouched();
      this.vendorPartyForm.get('name').markAsDirty();
      toastr.warning('Name is required');
      return;
    }
    if(this.vendorPartyForm.get('address').status === 'VALID') {
      this.vendorParty.address = this.vendorPartyForm.get('address').value;
    } else {
      this.vendorPartyForm.get('address').markAsTouched();
      this.vendorPartyForm.get('address').markAsDirty();
      toastr.warning('Address is required');
      return;
    }
    if(this.vendorPartyForm.get('email').status === 'VALID') {
      this.vendorParty.email = this.vendorPartyForm.get('email').value;
    } else {
      this.vendorPartyForm.get('email').markAsTouched();
      this.vendorPartyForm.get('email').markAsDirty();
      toastr.warning('Email is required, Enter valid email.');
      return;
    }
    if(this.vendorPartyForm.get('phone').status === 'VALID') {
      this.vendorParty.phone = this.vendorPartyForm.get('phone').value;
    } else {
      this.vendorPartyForm.get('phone').markAsTouched();
      this.vendorPartyForm.get('phone').markAsDirty();
      toastr.warning('Phone number is required.');
      return;
    }
    if(this.vendorPartyForm.get('website').status === 'VALID') {
      this.vendorParty.website = this.vendorPartyForm.get('website').value;
    } else {
      this.vendorPartyForm.get('website').markAsTouched();
      this.vendorPartyForm.get('website').markAsDirty();
      toastr.warning('website URL is required.');
      return;
    }
    if(this.vendorPartyForm.get('fax').status === 'VALID') {
      this.vendorParty.fax = this.vendorPartyForm.get('fax').value;
    } else {
      this.vendorPartyForm.get('fax').markAsTouched();
      this.vendorPartyForm.get('fax').markAsDirty();
      toastr.warning('Fax Number is required.');
      return;
    }
    if(this.vendorForm.get('payment').status === 'VALID') {
      this.vendor.paymentTermId = this.vendorForm.get('payment').value;
    } else {
      this.vendorForm.get('payment').markAsTouched();
      this.vendorForm.get('payment').markAsDirty();
      toastr.warning('Payment a/c is required.');
      return;
    }
    if(this.vendorForm.get('purchase').status === 'VALID') {
      this.vendor.purchaseAccountId = this.vendorForm.get('purchase').value;
    } else {
      this.vendorForm.get('purchase').markAsTouched();
      this.vendorForm.get('purchase').markAsDirty();
      toastr.warning('Purchase a/c is required.');
      return;
    }
    if(this.vendorForm.get('discount').status === 'VALID') {
      this.vendor.purchaseDiscountAccountId = this.vendorForm.get('discount').value;
    } else {
      this.vendorForm.get('discount').markAsTouched();
      this.vendorForm.get('discount').markAsDirty();
      toastr.warning('Discount a/c is required.');
      return;
    } if(this.partyContacts.length > 0) {
      this.vendorParty.contacts = this.partyContacts;
      this.vendor.party = this.vendorParty;
    } else {
      toastr.warning('Contacts are required, Add atleast 1 contact.');
      return;
    }
    return true;
  }
  // validation ends
  // initialization of Objects starts
  initObject() {
    this.vendor = {
      no: '',
      paymentTermId: 0,
      taxGroupId: 0,
      accountsPayableAccountId: 0,
      purchaseAccountId: 0,
      purchaseDiscountAccountId: 0,
      party:{
        name: '',
        address: '',
        email: '',
        phone: '',
        website: '',
        fax: '',
        contacts:[{
          firstName: '',
          lastName: '',
          middleName: '',
          isPrimary: '',
          id: ''
        }]
      }
    }
    this.vendorParty = {
      name: '',
      address: '',
      email: '',
      phone: '',
      website: '',
      fax: '',
      contacts:[{
        firstName: '',
        lastName: '',
        middleName: '',
        isPrimary: '',
        id: ''
      }]
    }
  }
  initForm() {
    this.vendorForm = new FormGroup({
      payment: new FormControl(null, Validators.required),
      purchase: new FormControl(null, Validators.required),
      discount: new FormControl(null, Validators.required)
    })
    this.vendorPartyForm = new FormGroup({
      name: new FormControl(null, Validators.required),
      address: new FormControl('', Validators.required),
      email: new FormControl(null,
        Validators.pattern('^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$')
      ),
      phone: new FormControl('', Validators.required),
      website: new FormControl('', Validators.required),
      fax: new FormControl('', Validators.required),
    });
    this.contactVendorParty = {
      firstName: '',
      lastName: '',
      middleName: '',
      isPrimary: '',
      id: ''
    }
  }
  initContactForm() {
    this.contactVendorPartyForm = new FormGroup({
      firstName: new FormControl(null, Validators.required),
      lastName: new FormControl(null, Validators.required),
      middleName: new FormControl(null, Validators.required),
      isPrimary: new FormControl(null, Validators.required),
      id: new FormControl(null, Validators.required)
    })
  }
  // initialization ends
}


