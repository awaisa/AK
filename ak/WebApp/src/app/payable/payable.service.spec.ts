import { TestBed, inject } from '@angular/core/testing';

import { PayableService } from './payable.service';

describe('PayableService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [PayableService]
    });
  });

  it('should ...', inject([PayableService], (service: PayableService) => {
    expect(service).toBeTruthy();
  }));
});
