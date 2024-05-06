class CreateApiV1Products < ActiveRecord::Migration[6.1]
  def change
    create_table :api_v1_products do |t|
      t.string :title
      t.text :content
      t.integer :prize

      t.timestamps
    end
  end
end
