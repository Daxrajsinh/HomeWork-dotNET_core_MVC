import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, NgForm, Validators } from '@angular/forms';

@Component({
  selector: 'react-form',
  templateUrl: './react-form.component.html',
  styleUrls: ['./react-form.component.css']
})
export class ReactFormComponent implements OnInit {

  user : FormGroup

  constructor() { }

  ngOnInit() {
    this.user = new FormGroup({
      name: new FormControl('John', [Validators.required]),
      email: new FormControl('', [Validators.email, Validators.required]),
      password: new FormControl('', [Validators.required]),
      address: new FormGroup({
        city: new FormControl(''),
        state: new FormControl(''),
        country: new FormControl('')
      })
    })
  }
  onSubmit(user) {
    console.log(user.value)
    console.log(user.valid)
    console.log(JSON.stringify(user.value))
  }
  loadFormWithData(){
    this.user.setValue({
      name: 'John',
      email: 'john@test.com',
      password: 'abcd',
      address: {
        city: 'Sydney',
        state: 'New South Wales',
        country: 'Australia'
      }
    })
  }
}
