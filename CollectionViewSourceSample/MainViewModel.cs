using CollectionViewSourceSample.Model;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace CollectionViewSourceSample
{
    public class MainViewModel : BindableBase
    {
        #region Properties
        /// <summary>
        /// 현재 입력된 검색 텍스트
        /// </summary>
        private string _currentSearchText { get; set; } = string.Empty;

        private double _allPrice;
        /// <summary>
        /// 총 금액
        /// </summary>
        public double AllPrice
        {
            get => _allPrice;
            set => SetProperty(ref _allPrice, value);
        }

        private CollectionViewSource _productCollection = new();
        /// <summary>
        /// 상품 컬렉션
        /// </summary>
        public CollectionViewSource ProductCollection
        {
            get => _productCollection;
            set => SetProperty(ref _productCollection, value);
        }

        private CollectionViewSource _cartItemCollection = new();
        /// <summary>
        /// 장바구니 아이템 컬렉션
        /// </summary>
        public CollectionViewSource CartItemCollection
        {
            get => _cartItemCollection;
            set => SetProperty(ref _cartItemCollection, value);
        }

        private ObservableCollection<Product> _products = new();
        /// <summary>
        /// 전체 상품 목록
        /// </summary>
        public ObservableCollection<Product> Products
        {
            get => _products;
            set => SetProperty(ref _products, value);
        }

        private ObservableCollection<Product> _cartItems = new();
        /// <summary>
        /// 전체 장바구니 상품 목록
        /// </summary>
        public ObservableCollection<Product> CartItems
        {
            get => _cartItems;
            set => SetProperty(ref _cartItems, value);
        }

        private string _productFilter = string.Empty;
        /// <summary>
        /// 상품 목록 필터
        /// </summary>
        public string ProductFilter
        {
            get => _productFilter;
            set => SetProperty(ref _productFilter, value);
        }

        private string _cartItemFilter = string.Empty;
        /// <summary>
        /// 장바구니 상품 목록 필터
        /// </summary>
        public string CartItemFilter
        {
            get => _cartItemFilter;
            set => SetProperty(ref _cartItemFilter, value);
        }

        private bool _isAllProductSelected;
        /// <summary>
        /// 상품 목록 전체선택 여부
        /// </summary>
        public bool IsAllProductSelected
        {
            get => _isAllProductSelected;
            set => SetProperty(ref _isAllProductSelected, value);
        }

        private bool _isAllCartItemSelected;
        /// <summary>
        /// 장바구니 상품 목록 전체선택 여부
        /// </summary>
        public bool IsAllCartItemSelected
        {
            get => _isAllCartItemSelected;
            set => SetProperty(ref _isAllCartItemSelected, value);
        }
        #endregion

        #region ICommand
        /// <summary>
        /// 검색 필터 커맨드
        /// </summary>
        public ICommand SearchFilterCommand { get; set; }

        /// <summary>
        /// 장바구니 추가 커맨드
        /// </summary>
        public ICommand AddCartCommand { get; set; }

        /// <summary>
        /// 장바구니 삭제 커맨드
        /// </summary>
        public ICommand RemoveCartCommand { get; set; }

        /// <summary>
        /// 전체선택 커맨드
        /// </summary>
        public ICommand AllSelectCommand { get; set; }

        /// <summary>
        /// 수량 빼기 커맨드
        /// </summary>
        public ICommand MinusCountCommand { get; set; }

        /// <summary>
        /// 수량 더하기 커맨드
        /// </summary>
        public ICommand AddCountCommand { get; set; }

        /// <summary>
        /// 마우스다운 커맨드
        /// </summary>
        public ICommand MouseDownCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            GetData();

            SearchFilterCommand = new DelegateCommand<string>(OnSearchFilter);
            AddCartCommand = new DelegateCommand(OnAddCart);
            RemoveCartCommand = new DelegateCommand(OnRemoveCart);
            AllSelectCommand = new DelegateCommand<string>(OnAllSelect);
            MinusCountCommand = new DelegateCommand<object>(OnMinusCount);
            AddCountCommand = new DelegateCommand<object>(OnAddCount);
            MouseDownCommand = new DelegateCommand<object>(OnMouseDown);
        }

        /// <summary>
        /// 마우스다운 커맨드 이벤트
        /// </summary>
        /// <remarks>수량이 1일 때 RepeatButton 클릭시 ListBoxItem 선택되는 현상 방지</remarks>
        /// <param name="obj"></param>
        private void OnMouseDown(object obj)
        {
            if (obj is MouseButtonEventArgs args)
            {
                args.Handled = true;
            }
        }

        /// <summary>
        /// 수량 더하기 커맨드 이벤트
        /// </summary>
        /// <param name="obj"></param>
        private void OnAddCount(object obj)
        {
            if (obj is Product product)
            {
                product.Count++;
                GetAllPrice();
            }    
        }

        /// <summary>
        /// 수량 빼기 커맨드 이벤트
        /// </summary>
        /// <param name="obj"></param>
        private void OnMinusCount(object obj)
        {
            if (obj is Product product)
            {
                product.Count--;
                GetAllPrice();
            }
        }

        /// <summary>
        /// 전체선택 커맨드 이벤트
        /// </summary>
        /// <param name="para"></param>
        private void OnAllSelect(string para)
        {
            switch (para)
            {
                case "Total":
                    _ = (from item in ProductCollection.View.OfType<Product>().Where(x => x.IsEnabled)
                         select item.IsProductSelected = IsAllProductSelected).Count();
                    break;
                case "Access":
                    _ = (from item in CartItemCollection.View.OfType<Product>()
                         select item.IsCartItemSelected = IsAllCartItemSelected).Count();
                    break;
            }
        }

        /// <summary>
        /// 초기 데이터 가져오기
        /// </summary>
        private void GetData()
        {
            List<Product> list = new();
            list.Add(new Product { Name = "Milk", Price = 3000, Count = 2, IsEnabled = false });
            list.Add(new Product { Name = "Candy", Price = 500, Count = 3, IsEnabled = false });
            list.Add(new Product { Name = "Water", Price = 700, Count = 1, IsEnabled = false });
            list.Add(new Product { Name = "Tissue", Price = 2000 });
            list.Add(new Product { Name = "Coke", Price = 1300 });
            list.Add(new Product { Name = "Coffee", Price = 2000 });
            list.Add(new Product { Name = "Cookie", Price = 1200 });

            Products.AddRange(list);
            ProductCollection.Source = Products;
            ProductCollection.Filter += SearchFilter;

            CartItems.AddRange(list.Where(x => !x.IsEnabled));
            CartItemCollection.Source = CartItems;
            CartItemCollection.Filter += SearchFilter;

            GetAllPrice();
        }

        /// <summary>
        /// 총 금액 계산
        /// </summary>
        private void GetAllPrice()
        {
            var prices = 0;
            CartItems.ToList().ForEach(x => prices += x.Price * x.Count);
            AllPrice = prices;
        }

        /// <summary>
        /// 장바구니 추가
        /// </summary>
        private void OnAddCart()
        {
            var added = Products.Where(x => x.IsProductSelected).ToList();
            CartItems.AddRange(added);

            _ = (from item in Products.Where(x => x.IsProductSelected)
                 let setCount = item.Count = 1
                 let setEnabled = item.IsEnabled = false
                 let setSelected = item.IsProductSelected = false
                 select item).Count();
            GetAllPrice();
        }

        /// <summary>
        /// 장바구니 삭제
        /// </summary>
        private void OnRemoveCart()
        {
            var removed = CartItems.Where(x => x.IsCartItemSelected).ToList();
            foreach (var item in removed)
            {
                CartItems.Remove(item);
            }

            _ = (from item in Products.Where(x => x.IsCartItemSelected)
                 let setEnabled = item.IsEnabled = true
                 let setSelected = item.IsCartItemSelected = false
                 select item).Count();
            GetAllPrice();
        }

        /// <summary>
        /// 필터 이벤트 연결
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchFilter(object sender, FilterEventArgs e)
        {
            e.Accepted = this.SearchFilter(e.Item);
        }

        /// <summary>
        /// 조건에 맞는 아이템 검색
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private bool SearchFilter(object item)
        {
            var product = item as Product;
            return product != null && product.Name.ToLower().Contains(this._currentSearchText.ToLower());
        }

        /// <summary>
        /// 검색 필터 커맨드 이벤트
        /// </summary>
        /// <param name="para"></param>
        private void OnSearchFilter(string para)
        {
            switch(para)
            {
                case "Total":
                    _currentSearchText = ProductFilter;
                    ProductCollection.View.Refresh();
                    break;
                case "Access":
                    _currentSearchText = CartItemFilter;
                    CartItemCollection.View.Refresh();
                    break;
            }
        }
    }
}
