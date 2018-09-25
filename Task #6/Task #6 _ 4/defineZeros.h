void equateElementsArray(int *ptr,int i,int n,int value){
  for(i = 0; i < n;i++)
    *(ptr + i) = value;
}
