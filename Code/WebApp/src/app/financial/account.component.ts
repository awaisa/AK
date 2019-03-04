import { Component, OnInit, Input } from '@angular/core';
import { ValidationErrorService } from '../shared/validation-error.service';
import { AppConfiguration } from '../business/appConfiguration';
import {UserInfo} from '../business/userInfo';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { slideInLeft, slideIn } from "../common/animations";

@Component({
  templateUrl: './account.component.html',
  animations: [slideIn]
})
export class AccountComponent implements OnInit {

  error: ValidationErrorService = new ValidationErrorService();
  loaded = false;
  aniFrame = 'in';
  @Input() hero: Hero;

  public hexadecimalValue: string;

  public dropdownValue: String = '';

  public isEditMode: Boolean = false;

  heroForm: FormGroup;
  nameChangeLog: string[] = [];
  states = states;

  constructor(private config: AppConfiguration, private user: UserInfo, private fb: FormBuilder) {

    this.createForm();
  }

  onSubmit() {

    this.heroForm.get('name').setErrors({error : 'this is custom error message'});

    const arrayControl = this.heroForm.get('secretLairs') as FormArray;
    for ( let i = 0; i < arrayControl.length; i++ ) {
      const formGroup = arrayControl.at(i);
      formGroup.get('state').setErrors({error : 'custom error message on state'});
    }

    alert(`Submit: ${JSON.stringify(this.heroForm.value)}`);
  }

  createForm() {
    this.heroForm = this.fb.group({
      name: ['awasdsds', Validators.required],
      secretLairs: this.fb.array([]),
      power: '',
      sidekick: ''
    });
  }
  
  ngOnChanges() {
    this.heroForm.reset({
      name: this.hero.name
    });
    this.setAddresses(this.hero.addresses);
  }

  get secretLairs(): FormArray {
    return this.heroForm.get('secretLairs') as FormArray;
  }

  setAddresses(addresses: Address[]) {
    const addressFGs = addresses.map(address => this.fb.group(address));
    const addressFormArray = this.fb.array(addressFGs);
    this.heroForm.setControl('secretLairs', addressFormArray);
  }

  addLair() {
    const addressfg = this.fb.group(new Address());
    addressfg.controls['city'].setValidators(Validators.required);
    addressfg.controls['state'].setValidators(Validators.required);
    this.secretLairs.push(addressfg);
  }

  prepareSaveHero(): Hero {
    const formModel = this.heroForm.value;

    // deep copy of form model lairs
    const secretLairsDeepCopy: Address[] = formModel.secretLairs.map(
      (address: Address) => Object.assign({}, address)
    );

    // return new `Hero` object containing a combination of original hero value(s)
    // and deep copies of changed form model values
    const saveHero: Hero = {
      id: this.hero.id,
      name: formModel.name as string,
      // addresses: formModel.secretLairs // <-- bad!
      addresses: secretLairsDeepCopy
    };
    return saveHero;
  }

  ngOnInit(): void {
    this.loaded = true;
  }
}

export class Hero {
  id = 0;
  name = '';
  addresses: Address[];
}

export class Address {
  street = '';
  city   = '';
  state  = '';
  zip    = '';
}

export const heroes: Hero[] = [
  {
    id: 1,
    name: 'Whirlwind',
    addresses: [
      {street: '123 Main',  city: 'Anywhere', state: 'CA',  zip: '94801'},
      {street: '456 Maple', city: 'Somewhere', state: 'VA', zip: '23226'},
    ]
  },
  {
    id: 2,
    name: 'Bombastic',
    addresses: [
      {street: '789 Elm',  city: 'Smallville', state: 'OH',  zip: '04501'},
    ]
  },
  {
    id: 3,
    name: 'Magneta',
    addresses: [ ]
  },
];

export const states = [{id: 1, name: 'CA'}, {id: 2, name: 'MD'}, {id: 3, name: 'OH'}, {id: 4, name: 'VA'}];
