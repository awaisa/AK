import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import { FinancialService } from '../financial.service';
import { ValidationErrorService } from "../../shared/validation-error.service";
import { RefService } from '../../shared/ref.service';
import { PaymentTerm, TaxGroup, 
  Customer, 
  Party, 
  Contact, 
  Account } from '../../entities';

import { AppConfiguration } from '../../business/appConfiguration';
import { TitleService } from '../../shared/title.service';

@Component({
  templateUrl: './journal-entry.component.html'
})
export class JournalEntryComponent implements OnInit {

  isNew = false;
  isEditMode = false;
  
  title: string;

  loaded = false;
  aniFrame = 'in';
  errorMessages: string[];
  error: ValidationErrorService = new ValidationErrorService();
  formGroup: FormGroup;
  
  accounts: Account[] = [];
  taxgroups: TaxGroup[] = [];
  paymentTerms: PaymentTerm[] = [];

  customer: Customer = new Customer(); 

  addContactFlag = false;

  constructor(private route: ActivatedRoute, private fb: FormBuilder,
    private router: Router,
    private financialService: FinancialService,
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
      lineItems: this.fb.array([]),
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
      this.financialService.getJournalEntry(id)
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
      this.error.focusControl(this.formGroup);
    }
    this.error.reset();
  }

  save() {
    this.errorMessages = [];
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
      this.financialService.saveJournalEntry(customer)
        .subscribe((customer: Customer) => {
          this.customer = customer;
          var msg = customer.party.name + " has been saved."
          this.error.info(msg);
          window.location.hash = "receivable/customers/" + customer.id;
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
                this.errorMessages.push(validationErrorDictionary[fieldName]);
              }
            }
            this.error.error(this.errorMessages.join('\n'));
            this.error.focusInvalidControl(this.formGroup);
          } else {
            this.error.error(err);
          }
        });
      this.formGroup.markAsPristine();
      this.formGroup.markAsUntouched();
      this.formGroup.updateValueAndValidity();      
    } else {
      this.error.focusInvalidControl(this.formGroup);
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
        this.error.focusInvalidControl(this.partyContacts.controls[i] as FormGroup);
        return;
      }
    }

    let contact = new Contact();
    if(this.partyContacts.length == 0) contact.isPrimary = true;
    const g = this.fb.group(contact);
    this.setContactValidator(g);
    this.partyContacts.push(g);
    
    this.error.focusControl(g);
  }

  setContactValidator(fg: FormGroup) {
    fg.get("firstName").setValidators([Validators.required]);
    //fg.get("lastName").setValidators([Validators.required]);
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
