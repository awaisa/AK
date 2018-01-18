import {Component, OnInit, ElementRef, Input} from '@angular/core';
import {Router, ActivatedRoute} from "@angular/router";
import { FormControl, FormGroup, Validators } from '@angular/forms';

//declare var $:any ;
declare var $:any;
declare var toastr:any;
declare var window:any;

@Component({
  selector: 'app-add-customer-component',
  templateUrl: './add-customer.component.html',
  styleUrls: ['./add-customer.component.css']
})
export class AddCustomerComponent implements OnInit {
@Input() close;
@Input() submit;

  constructor(private route: ActivatedRoute,
    private router: Router) {
}

  customerForm: FormGroup;
  album:album;
  albums: Array<any> = [];


  enableEditId = '';

  ngOnInit() {
 this.initForm();
  }

  initForm() {
    this.customerForm = new FormGroup ({
      name: new FormControl(null,Validators.required),
      brand_name: new FormControl('',Validators.required),
      album_desc: new FormControl('',Validators.required),
      img_url: new FormControl('',Validators.required),
      purchase_url: new FormControl('',Validators.required),
      listen_url: new FormControl('',Validators.required),
      year: new FormControl('',Validators.required)
    });
    this.album = {
      Id:'',
      Title: '',
      Year: '',
      Artist: {
        ArtistName: '',
        Description: '',
      },
      Description: ''
    }
  }
  edit(c) {
    this.album = this.albums.filter(x => x.Id == c)[0]
    this.enableEditId = c;
  }
  cancel(){
    this.close();
  }
  add() {
    if(this.validateForm()) {
      this.submit(this.album);
      this.close()
    }
  }

  validateForm() {
    if(this.customerForm.get('name').status === 'VALID') {
      this.album.Title = this.customerForm.get('name').value;
    } else {
      this.customerForm.get('name').markAsTouched();
      this.customerForm.get('name').markAsDirty();
      toastr.warning('Name is required');
      return
    }
    if(this.customerForm.get('brand_name').status === 'VALID') {
      this.album.Artist.ArtistName = this.customerForm.get('brand_name').value;
    } else {
      this.customerForm.get('brand_name').markAsTouched();
      this.customerForm.get('brand_name').markAsDirty();
      toastr.warning('Artist Name is required');
      return
    }
    if(this.customerForm.get('album_desc').status === 'VALID') {
      this.album.Description = this.customerForm.get('album_desc').value;
    } else {
      this.customerForm.get('album_desc').markAsTouched();
      this.customerForm.get('album_desc').markAsDirty();
      toastr.warning('Description is required');
      return
    }
    if(this.customerForm.get('img_url').status === 'VALID') {
    } else {
      this.customerForm.get('img_url').markAsTouched();
      this.customerForm.get('img_url').markAsDirty();
      toastr.warning('Image is required');
      return
    }
    if(this.customerForm.get('purchase_url').status === 'VALID') {
    } else {
      this.customerForm.get('purchase_url').markAsTouched();
      this.customerForm.get('purchase_url').markAsDirty();
      toastr.warning('Purchase is required');
      return
    }
    if(this.customerForm.get('listen_url').status === 'VALID') {
    } else {
      this.customerForm.get('listen_url').markAsTouched();
      this.customerForm.get('listen_url').markAsDirty();
      toastr.warning('Listen URL is required');
      return
    }
    if(this.customerForm.get('year').status === 'VALID') {
      this.album.Year = this.customerForm.get('year').value;
    } else {
      this.customerForm.get('year').markAsTouched();
      this.customerForm.get('year').markAsDirty();
      toastr.warning('Year is required');
      return
    }
    return true;
  }
}

interface album {
  Id:'',
  Title: '',
  Year: '',
  Artist: {
    ArtistName: '',
    Description: '',
  },
  Description: ''
}
