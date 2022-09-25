using Prism.Mvvm;

namespace CollectionViewSourceSample.Model
{
    /// <summary>
    /// 상품 모델
    /// </summary>
    public class Product : BindableBase
    {
        private string _name;
        /// <summary>
        /// 상품명
        /// </summary>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int _price;
        /// <summary>
        /// 상품 가격
        /// </summary>
        public int Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        private int _count = 1;
        /// <summary>
        /// 선택 개수
        /// </summary>
        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        private bool _isProductSelected = false;
        /// <summary>
        /// 상품 선택 여부
        /// </summary>
        public bool IsProductSelected
        {
            get => _isProductSelected;
            set => SetProperty(ref _isProductSelected, value);
        }

        private bool _isCartItemSelected = false;
        /// <summary>
        /// 장바구니 상품 선택 여부
        /// </summary>
        public bool IsCartItemSelected
        {
            get => _isCartItemSelected;
            set => SetProperty(ref _isCartItemSelected, value);
        }

        private bool _isEnabled = true;
        /// <summary>
        /// 선택 가능 여부 (리스트박스)
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }
    }
}
