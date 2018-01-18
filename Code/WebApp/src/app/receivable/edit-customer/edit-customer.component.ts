import {Component, OnInit, ElementRef} from '@angular/core';
import {Router, ActivatedRoute} from "@angular/router";
import { FormControl, FormGroup, Validators } from '@angular/forms';

//declare var $:any ;
declare var $:any;
declare var toastr:any;
declare var window:any;

@Component({
  selector: 'app-edit-customer',
  templateUrl: './edit-customer.component.html',
  styleUrls: ['./edit-customer.component.css']
})
export class EditCustomerComponent implements OnInit {

  constructor(private route: ActivatedRoute,
    private router: Router) {
}

  loaded = false;
  aniFrame = 'in';

  customerForm: FormGroup;
  album:any = {};
  albums: Array<any> = [];

  enablePopup  = false;
  enableEditId = '';

  ngOnInit() {
    this.albums[0] =JSON.parse('{"Id":4,"ArtistId":4,"Title":"Alchemy index, Vols. 3-4","Description":"Post-hardcore foursome Thrice seems to revel in pushing boundaries and overturning expectations. The second two-disc installment of THE ALCHEMY INDEX answers VOLS. 1 & 2 (FIRE & WATER) from the first set with VOLS. 3 & 4 (EARTH & AIR). There’s little evidence of the ferocious screamo aesthetic that characterized releases like THE ILLUSION OF SAFETY; instead AIR & EARTH ventures into mostly acoustic, highly melodic territory. The tight, technically complex instrumental prowess of the band is still on display, but the mood is much more atmospheric. The songs veer from ethereal and psychedelic to a more conventional folk or singer-songwriter vein (this is especially true of the tracks on the EARTH disc), but the whole adds up to an impressive, original showing from one of the genre’s innovators.","Year":2008,"ImageUrl":"https://images-na.ssl-images-amazon.com/images/I/61AY91dfLAL._SL250_.jpg","AmazonUrl":"http://www.amazon.com/gp/product/B0015FS8QC/ref=as_li_tl?ie=UTF8&camp=1789&creative=390957&creativeASIN=B0015FS8QC&linkCode=as2&tag=westwindtechn-20&linkId=2JW432WUQFTHXXBH","SpotifyUrl":"https://play.spotify.com/album/2FNlKycIizpYWMID16RMzO","Artist":{"Id":4,"ArtistName":"Thrice","Description":"Thrice is an American rock band from Irvine, California, formed in 1998. The group was founded by guitarist/vocalist Dustin Kensrue and guitarist Teppei Teranishi while they were in high school.[1]Early in their career, the band was known for fast, hard music based in heavily distorted guitars, prominent lead guitar lines, and frequent changes in complex time signatures.[2] This style is exemplified on their second album, The Illusion of Safety (2002) and their third album The Artist in the Ambulance (2003). Their fourth album Vheissu (2005) made significant changes by incorporating electronic beats, keyboards, and more experimental and nuanced songwriting.[3][4] Their fifth effort was a double album entitled The Alchemy Index (2007/2008), released as two sets of two CDs that together make a 4-part, 24-song cycle. Each of the four 6-song EPs of the Alchemy Index features significantly different styles, based on different aspects of the bands musical aesthetic which reflect the elemental themes of fire, water, air and earth, both lyrically and musically.[5] The bands sixth album, entitled Beggars, was released on August 11, 2009, and their seventh, Major/Minor on September 20, 2011. The most recent albums feature a refined combination of the bands different experiments and explorations.","ImageUrl":"http://images.onset.freedom.com/ocregister/blogs/soundcheck.ocregister.com/thrice-hob_anaheim_ACY7764.jpg","AmazonUrl":""},"Tracks":[{"Id":16,"AlbumId":4,"SongName":"Broken Lungs","Length":"4:14","Bytes":0,"UnitPrice":0.0},{"Id":17,"AlbumId":4,"SongName":"The Sky is falling","Length":"4:21","Bytes":0,"UnitPrice":0.0},{"Id":18,"AlbumId":4,"SongName":"Moving Mountains","Length":"2:55","Bytes":0,"UnitPrice":0.0}]}');  
    this.initAlbum();
    this.openPopup = this.openPopup.bind(this);
    this.add = this.add.bind(this);
  }
  initAlbum() {
    this.album = {
      id:'',
      Title: '',
      Year: '',
      Artist: {
        ArtistName: '',
        Description: '',
      },
      Description: ''
    };
  }
  initForm() {
    this.customerForm = new FormGroup ({
      name: new FormControl(null,Validators.required),
      brand_name: new FormControl('',Validators.required),
      album_desc: new FormControl('',Validators.required),
      img_url: new FormControl('',Validators.required),
      purchase_url: new FormControl('',Validators.required),
      listen_url: new FormControl('',Validators.required),
      release_url: new FormControl('',Validators.required)
    });
  }
  edit(c) {
    this.album = this.albums.filter(x => x.Id == c)[0]
    this.enableEditId = c;
  }
  cancel(){
    this.enableEditId = '';
    this.initAlbum();
  }
  add(album) {
    album.Id = Math.random();
    this.albums.push(album);
    this.initAlbum();
  }
  submitForm() {
    let index = this.albums.indexOf(x => x.Id === this.enableEditId)
    this.albums[index] = this.album;
    this.enableEditId = '';
  }
  openPopup() {
    this.enablePopup = !this.enablePopup;
  }
  validateForm() {
    if(this.customerForm.get('name').status === 'VALID') {
      return true;
    } else {
      this.customerForm.get('name').markAsDirty();
      toastr.warning('Name is required');
    }
  }
}

interface customer {
  name: string;
  brandName: string;
  albumDescription: string;
  imageUrl: string;
  purchaseUrl: string;
  listenUrl: string;
  releaseYear: string;
}
