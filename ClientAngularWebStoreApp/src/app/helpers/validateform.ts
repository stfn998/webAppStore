import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn } from "@angular/forms";

export default class ValidateForm{

    static validateAllFormFiels(formGroup: FormGroup){
        Object.keys(formGroup.controls).forEach(field =>{
          const control = formGroup.get(field);
          if (control instanceof FormControl){
            control.markAsDirty({onlySelf:true});
          }else if(control instanceof FormGroup){
            this.validateAllFormFiels(control)
          }
        })
    }
      
    
    static confirmedValidator(source: string, target: string): ValidatorFn {
        return (control: AbstractControl): ValidationErrors | null => {
          const sourceCtrl = control.get(source);
          const targetCtrl = control.get(target);
    
          return sourceCtrl?.value !== targetCtrl?.value
            ? { passwordMatchError: true }
            : null;
        };
    }
}