class Api::V1::ProductsController < ApplicationController
  before_action :set_api_v1_product, only: %i[ show edit update destroy ]

  # GET /api/v1/products or /api/v1/products.json
  def index
    @api_v1_products = Api::V1::Product.all
  end

  # GET /api/v1/products/1 or /api/v1/products/1.json
  def show
  end

  # GET /api/v1/products/new
  def new
    @api_v1_product = Api::V1::Product.new
  end

  # GET /api/v1/products/1/edit
  def edit
  end

  # POST /api/v1/products or /api/v1/products.json
  def create
    @api_v1_product = Api::V1::Product.new(api_v1_product_params)

    respond_to do |format|
      if @api_v1_product.save
        format.html { redirect_to api_v1_product_url(@api_v1_product), notice: "Product was successfully created." }
        format.json { render :show, status: :created, location: @api_v1_product }
      else
        format.html { render :new, status: :unprocessable_entity }
        format.json { render json: @api_v1_product.errors, status: :unprocessable_entity }
      end
    end
  end

  # PATCH/PUT /api/v1/products/1 or /api/v1/products/1.json
  def update
    respond_to do |format|
      if @api_v1_product.update(api_v1_product_params)
        format.html { redirect_to api_v1_product_url(@api_v1_product), notice: "Product was successfully updated." }
        format.json { render :show, status: :ok, location: @api_v1_product }
      else
        format.html { render :edit, status: :unprocessable_entity }
        format.json { render json: @api_v1_product.errors, status: :unprocessable_entity }
      end
    end
  end

  # DELETE /api/v1/products/1 or /api/v1/products/1.json
  def destroy
    @api_v1_product.destroy

    respond_to do |format|
      format.html { redirect_to api_v1_products_url, notice: "Product was successfully destroyed." }
      format.json { head :no_content }
    end
  end

  private
    # Use callbacks to share common setup or constraints between actions.
    def set_api_v1_product
      @api_v1_product = Api::V1::Product.find(params[:id])
    end

    # Only allow a list of trusted parameters through.
    def api_v1_product_params
      params.require(:api_v1_product).permit(:title, :content, :prize)
    end
end
