import { Component, OnInit, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder, FormControl, FormGroup, Validators, FormArray, FormControlDirective, FormControlName } from '@angular/forms';
import { ReceivableService } from '../receivable.service';
import { ErrorInfo } from "../../shared/ErrorInfo";
import { RefService } from '../../shared/ref.service';
import { PaymentTerm, TaxGroup, 
  Customer, 
  Party, 
  Contact, 
  Account } from '../../entities';

import { AppConfiguration } from '../../business/appConfiguration';
import { TitleService } from '../../shared/title.service';

declare var $: any;
declare var window: any;

//https://stackoverflow.com/a/44963270/2123712
const originFormControlNgOnChanges = FormControlDirective.prototype.ngOnChanges;
FormControlDirective.prototype.ngOnChanges = function () {
  this.form.nativeElement = this.valueAccessor._elementRef.nativeElement;
  return originFormControlNgOnChanges.apply(this, arguments);
};
const originFormControlNameNgOnChanges = FormControlName.prototype.ngOnChanges;
FormControlName.prototype.ngOnChanges = function () {
  const result = originFormControlNameNgOnChanges.apply(this, arguments);
  this.control.nativeElement = this.valueAccessor._elementRef.nativeElement;
  return result;
};

@Component({
  templateUrl: './edit-customer.component.html'
})
export class EditCustomerComponent implements OnInit {

  isNew = false;
  isEditMode = false;
  
  title: string;

  loaded = false;
  aniFrame = 'in';
  errors: string[];
  error: ErrorInfo = new ErrorInfo();
  formGroup: FormGroup;
  
  accounts: Account[] = [];
  taxgroups: TaxGroup[] = [];
  paymentTerms: PaymentTerm[] = [];

  customer: Customer = new Customer(); 

  addContactFlag = false;

  constructor(private route: ActivatedRoute, private fb: FormBuilder,
    private router: Router,
    private receivableService: ReceivableService,
    private refService: RefService,
    private config: AppConfiguration,
    private titleService: TitleService
  ) {}

  ngOnInit() {
    var id = this.route.snapshot.params["id"];
    this.formGroup = this.fb.group({
      id: [id],
      no: [''],
      name: ['', Validators.required],
      address: ['', Validators.required],
      email: ['', Validators.required],
      phone: [''],
      website: [''],
      fax: [''],
      contacts: this.fb.array([]),
      paymentTermId: ['', Validators.required],
      taxGroupId: ['', Validators.required],

      accountsReceivableAccountId: ['', Validators.required],
      salesAccountId: ['', Validators.required],
      salesDiscountAccountId: ['', Validators.required],
      promptPaymentDiscountAccountId: ['', Validators.required],
      customerAdvancesAccountId: ['', Validators.required],
    });
    
    this.bindLists();

    if (id > 0) {
      this.receivableService.getCustomer(id)
        .subscribe(result => {
          this.customer = result;
          this.reset();
          this.loaded = true;
        },
        err => {
          this.error.error(err);
        });
    } else {
      this.isEditMode = true;
      this.isNew = true;
    }    

    this.onChanges();
  }

  onChanges(): void {
    this.formGroup.valueChanges.subscribe(val => {
      this.title = `Customer ${(val.name == null || val.name == '') ? '' : '- ' +val.name }`;
      this.titleService.setTitle(this.title);
    });
  }

  editModeChange(event) {
    this.isEditMode = event;
    if(!event) {
      this.reset();
    }else {
      setTimeout(()=>{
        this.focusControl(this.formGroup);
      }, 100);
    }
    this.error.reset();
  }

  save() {
    this.errors = [];
    this.error.reset();
    
    if (this.formGroup.valid) {

      // deep copy of form model lairs
      const deepCopyContacts: Contact[] = this.formGroup.value.contacts.map((contact: Contact) => Object.assign({}, contact));

      let customer = {
        id: this.customer.id,
        no: this.formGroup.value.no,
        paymentTermId: this.formGroup.value.paymentTermId,
        taxGroupId: this.formGroup.value.taxGroupId,
        
        accountsReceivableAccountId: this.formGroup.value.accountsReceivableAccountId,
        salesAccountId: this.formGroup.value.salesAccountId,
        salesDiscountAccountId: this.formGroup.value.salesDiscountAccountId,
        promptPaymentDiscountAccountId: this.formGroup.value.promptPaymentDiscountAccountId,
        customerAdvancesAccountId: this.formGroup.value.customerAdvancesAccountId,
        party: {
          name: this.formGroup.value.name,
          address: this.formGroup.value.address,
          email: this.formGroup.value.email,
          phone: this.formGroup.value.phone,
          website: this.formGroup.value.website,
          fax: this.formGroup.value.fax,
          contacts: deepCopyContacts
       }
      };
      this.receivableService.saveCustomer(customer)
        .subscribe((customer: Customer) => {
          this.customer = customer;
          var msg = customer.party.name + " has been saved."
          this.error.info(msg);
          setTimeout(function () {
            window.location.hash = "receivable/customers/" + customer.id;
          }, 500)
        },
        err => {
          if (err.response.status === 400) {
            // handle validation error
            let validationErrorDictionary = JSON.parse(err.response._body);
            for (var fieldName in validationErrorDictionary) {
              if (this.formGroup.controls[fieldName]) {
                const msg = validationErrorDictionary[fieldName];
                // integrate into angular's validat ion if we have field validation
                this.formGroup.controls[fieldName].setErrors({ error: msg });
              } else {
                if(fieldName.indexOf('.') >= 0) {
                  
                }
                // if we have cross field validation then show the validation error at the top of the screen
                this.errors.push(validationErrorDictionary[fieldName]);
              }
            }
            this.error.error(this.errors.join('\n'));
            this.focusInvalidControl(this.formGroup);
          } else {
            this.error.error(err);
          }
        });
      this.formGroup.markAsPristine();
      this.formGroup.markAsUntouched();
      this.formGroup.updateValueAndValidity();      
    } else {
      this.focusInvalidControl(this.formGroup);
    }
  }

  reset(){
    this.formGroup.reset({
      no: this.customer.no,
      paymentTermId: this.customer.paymentTermId,
      taxGroupId: this.customer.taxGroupId,

      accountsReceivableAccountId: this.customer.accountsReceivableAccountId,
      salesAccountId: this.customer.salesAccountId,
      salesDiscountAccountId: this.customer.salesDiscountAccountId,
      promptPaymentDiscountAccountId: this.customer.promptPaymentDiscountAccountId,
      customerAdvancesAccountId: this.customer.customerAdvancesAccountId,

      name: this.customer.party.name,
      address: this.customer.party.address,
      email: this.customer.party.email,
      phone: this.customer.party.phone,
      website: this.customer.party.website,
      fax: this.customer.party.fax,
    });
    this.setPartyContacts(this.customer.party.contacts);
  }

  focusInvalidControl(fg: FormGroup) {
    let invalid = <any[]>Object.keys(fg.controls).map(key => fg.controls[key]).filter(ctl => ctl.invalid);
    if (invalid.length > 0) {
      if('controls' in invalid[0]) {
        for(var i=0; i<invalid[0].controls.length; i++) {
          if(!invalid[0].controls[i].valid) {
            this.focusInvalidControl(invalid[0].controls[i] as FormGroup);
          }
        }
      } else {
        $((<any>invalid[0]).nativeElement).find('input,select').focus();
      }
    }
  }

  focusControl(fg: FormGroup) {
    let ctrls = <any[]>Object.keys(fg.controls).map(key => fg.controls[key]);
    if (ctrls.length > 0) {
      if('controls' in ctrls[0]) {
        this.focusInvalidControl(ctrls[0].controls[0] as FormGroup);
      } else {
        for(var i=0; i<ctrls.length; i++) {
          const element = (<any>ctrls[i]).nativeElement;
          if(element != null) {
            $(element).find('input,select').focus();
            return;
          }
        }
      }
    }
  }

  setPartyContacts(contacts: Contact[]) {
    const contactFGs = contacts.map(c => {
      const fg = this.fb.group(c);
      this.setContactValidator(fg);
      return fg;
    });
    const contactFormArray = this.fb.array(contactFGs);
    this.formGroup.setControl('contacts', contactFormArray);
  }

  addContact() {
    
    for(var i=0; i<this.partyContacts.length; i++) {
      if(!this.partyContacts.controls[i].valid) {
        this.focusInvalidControl(this.partyContacts.controls[i] as FormGroup);
        return;
      }
    }

    let contact = new Contact();
    if(this.partyContacts.length == 0) contact.isPrimary = true;
    const g = this.fb.group(contact);
    this.setContactValidator(g);
    this.partyContacts.push(g);
    
    setTimeout(()=>{
      this.focusControl(g);
    }, 100);
  }

  setContactValidator(fg: FormGroup) {
    fg.get("firstName").setValidators([Validators.required]);
    fg.get("lastName").setValidators([Validators.required]);
    fg.get("middleName").setValidators([Validators.required]);
  }

  deleteContact(i) {
    var c = this.partyContacts[i];
    this.partyContacts.removeAt(i);
  }

  get partyContacts(): FormArray {
    return this.formGroup.get('contacts') as FormArray;
  };

  contactIsPrimaryChange(event){
    for(var i=0; i<this.partyContacts.length; i++){
      var c = this.partyContacts.controls[i];
      var dd = c.get("isPrimary");
      dd.setValue(false);
    }
  }

  bindLists(){
    this.refService.getTaxGroups()
      .subscribe(result => {
        this.taxgroups = result;
      },
      err => {
        this.error.error(err);
      });
    this.refService.getAccounts()
      .subscribe(result => {
        this.accounts = result;
      },
      err => {
        this.error.error(err);
      });
    this.refService.getPaymentTerms()
      .subscribe(result => {
        this.paymentTerms = result;
      },
      err => {
        this.error.error(err);
      });
  }
}
